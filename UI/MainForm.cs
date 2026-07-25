using AudioNormPlus.Models;
using AudioNormPlus.Services;

namespace AudioNormPlus.UI
{
    public partial class MainForm : Form
    {
        private List<AudioFile> audioFiles = new();
        private AnalysisMode currentMode = AnalysisMode.Track;
        private AudioAnalyzer analyzer = new();
        private ReplayGainCalculator calculator = new();
        private GainApplier applier = new();
        private CancellationTokenSource? cancellationTokenSource;

        private DataGridView? fileGrid;
        private TrackBar? gainSlider;
        private Label? gainValueLabel;

        public MainForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            // Form properties
            Text = "AudioNorm+ - Replay Gain Processor v1.0.0";
            Size = new Size(1000, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = SystemColors.Control;
            Icon = SystemIcons.Application;

            // Main panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = SystemColors.Control
            };
            Controls.Add(mainPanel);

            // Title label
            Label titleLabel = new Label
            {
                Text = "AudioNorm+ - Replay Gain Processor",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(10, 5),
                AutoSize = true,
                ForeColor = SystemColors.ControlText
            };
            mainPanel.Controls.Add(titleLabel);

            // Top control panel
            Panel controlPanel = new Panel
            {
                Height = 140,
                Dock = DockStyle.Top,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = SystemColors.ControlLightLight,
                Margin = new Padding(0, 5, 0, 5)
            };
            mainPanel.Controls.Add(controlPanel);

            // Analysis mode selection
            Label modeLabel = new Label
            {
                Text = "Analysis Mode:",
                Location = new Point(15, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = SystemColors.ControlText
            };
            controlPanel.Controls.Add(modeLabel);

            RadioButton trackModeRadio = new RadioButton
            {
                Text = "Track (Individual)",
                Location = new Point(130, 15),
                Width = 150,
                Checked = true,
                Font = new Font("Segoe UI", 10),
                ForeColor = SystemColors.ControlText
            };
            trackModeRadio.CheckedChanged += (s, e) => { if (trackModeRadio.Checked) currentMode = AnalysisMode.Track; };
            controlPanel.Controls.Add(trackModeRadio);

            RadioButton albumModeRadio = new RadioButton
            {
                Text = "Album (Collective)",
                Location = new Point(290, 15),
                Width = 150,
                Font = new Font("Segoe UI", 10),
                ForeColor = SystemColors.ControlText
            };
            albumModeRadio.CheckedChanged += (s, e) => { if (albumModeRadio.Checked) currentMode = AnalysisMode.Album; };
            controlPanel.Controls.Add(albumModeRadio);

            // Buttons row 1
            Button addFilesBtn = new Button
            {
                Text = "Add Files",
                Location = new Point(15, 45),
                Width = 90,
                Height = 32,
                BackColor = SystemColors.Control,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            addFilesBtn.Click += AddFiles_Click;
            controlPanel.Controls.Add(addFilesBtn);

            Button analyzeBtn = new Button
            {
                Text = "Analyze",
                Location = new Point(110, 45),
                Width = 90,
                Height = 32,
                BackColor = SystemColors.Control,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            analyzeBtn.Click += Analyze_Click;
            controlPanel.Controls.Add(analyzeBtn);

            Button clearBtn = new Button
            {
                Text = "Clear All",
                Location = new Point(205, 45),
                Width = 90,
                Height = 32,
                BackColor = SystemColors.Control,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            clearBtn.Click += Clear_Click;
            controlPanel.Controls.Add(clearBtn);

            // Gain adjustment section
            Label gainLabel = new Label
            {
                Text = "Gain Adjustment (dB):",
                Location = new Point(15, 85),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = SystemColors.ControlText
            };
            controlPanel.Controls.Add(gainLabel);

            gainSlider = new TrackBar
            {
                Location = new Point(160, 82),
                Width = 250,
                Height = 35,
                Minimum = -48,
                Maximum = 48,
                Value = 0,
                TickStyle = TickStyle.BottomRight,
                TickFrequency = 2
            };
            gainSlider.ValueChanged += GainSlider_ValueChanged;
            controlPanel.Controls.Add(gainSlider);

            gainValueLabel = new Label
            {
                Text = "0.0 dB",
                Location = new Point(420, 90),
                Width = 50,
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            controlPanel.Controls.Add(gainValueLabel);

            Button applyBtn = new Button
            {
                Text = "Apply Gain",
                Location = new Point(480, 45),
                Width = 90,
                Height = 32,
                BackColor = Color.LightGreen,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            applyBtn.Click += ApplyGain_Click;
            controlPanel.Controls.Add(applyBtn);

            // Status label
            Label statusLabel = new Label
            {
                Text = "Ready",
                Location = new Point(15, 115),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.DarkGreen
            };
            controlPanel.Controls.Add(statusLabel);

            // File list grid
            fileGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = SystemColors.Window,
                BorderStyle = BorderStyle.Fixed3D,
                RowHeadersVisible = true,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            };

            fileGrid.Columns.AddRange(
                new DataGridViewTextBoxColumn
                {
                    Name = "FileName",
                    HeaderText = "File Name",
                    Width = 280,
                    DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 9) }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Format",
                    HeaderText = "Format",
                    Width = 70,
                    DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 9), Alignment = DataGridViewContentAlignment.MiddleCenter }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Duration",
                    HeaderText = "Duration",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 9), Alignment = DataGridViewContentAlignment.MiddleCenter }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Loudness",
                    HeaderText = "Loudness (LUFS)",
                    Width = 130,
                    DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 9), Alignment = DataGridViewContentAlignment.MiddleCenter }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "CalculatedGain",
                    HeaderText = "Calc. Gain (dB)",
                    Width = 120,
                    DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 9), Alignment = DataGridViewContentAlignment.MiddleCenter }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "AppliedGain",
                    HeaderText = "Applied Gain (dB)",
                    Width = 130,
                    DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 9), Alignment = DataGridViewContentAlignment.MiddleCenter }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Status",
                    HeaderText = "Status",
                    Width = 110,
                    DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 9), Alignment = DataGridViewContentAlignment.MiddleCenter }
                }
            );

            mainPanel.Controls.Add(fileGrid);
            fileGrid.BringToFront();
        }

        private void GainSlider_ValueChanged(object? sender, EventArgs e)
        {
            if (gainSlider != null && gainValueLabel != null)
            {
                double gainDb = gainSlider.Value * 0.5;
                gainValueLabel.Text = $"{gainDb:+0.0;-0.0;0.0} dB";
            }
        }

        private void AddFiles_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "Audio Files (*.mp3;*.aac;*.m4a)|*.mp3;*.aac;*.m4a|MP3 Files (*.mp3)|*.mp3|AAC Files (*.aac;*.m4a)|*.aac;*.m4a|All Files (*.*)|*.*";
                openDialog.Multiselect = true;
                openDialog.Title = "Select Audio Files";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in openDialog.FileNames)
                    {
                        if (!audioFiles.Any(f => f.FilePath == filePath))
                        {
                            audioFiles.Add(new AudioFile(filePath));
                        }
                    }

                    UpdateFileGrid();
                }
            }
        }

        private async void Analyze_Click(object? sender, EventArgs e)
        {
            if (!audioFiles.Any())
            {
                MessageBox.Show("Please add audio files first.", "No Files", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                cancellationTokenSource = new CancellationTokenSource();

                foreach (var file in audioFiles)
                {
                    if (cancellationTokenSource.Token.IsCancellationRequested)
                        break;

                    await analyzer.AnalyzeFileAsync(file);
                    UpdateFileGrid();
                }

                // Calculate gains based on mode
                if (currentMode == AnalysisMode.Track)
                {
                    foreach (var file in audioFiles)
                    {
                        if (file.LoudnessIntegrated.HasValue)
                        {
                            double gain = calculator.CalculateTrackGain(file);
                            file.CalculatedGain = calculator.NormalizeGainIncrement(gain);
                        }
                    }
                }
                else // Album mode
                {
                    double albumGain = calculator.CalculateAlbumGain(audioFiles);
                    albumGain = calculator.NormalizeGainIncrement(albumGain);

                    foreach (var file in audioFiles)
                    {
                        file.CalculatedGain = albumGain;
                    }
                }

                UpdateFileGrid();
                MessageBox.Show("Analysis complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Analysis failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ApplyGain_Click(object? sender, EventArgs e)
        {
            if (!audioFiles.Any(f => f.Status == ProcessingStatus.Analyzed || f.Status == ProcessingStatus.Applied))
            {
                MessageBox.Show("Please analyze files first.", "No Analysis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (gainSlider == null)
                return;

            double gainDb = gainSlider.Value * 0.5;

            try
            {
                if (currentMode == AnalysisMode.Track)
                {
                    foreach (var file in audioFiles)
                    {
                        await applier.ApplyGainAsync(file, gainDb);
                    }
                }
                else // Album mode
                {
                    var filesToProcess = audioFiles.Where(f => f.Status == ProcessingStatus.Analyzed || f.Status == ProcessingStatus.Applied).ToList();
                    await applier.ApplyAlbumGainAsync(filesToProcess, gainDb, gainDb);
                }

                UpdateFileGrid();
                MessageBox.Show("Gain applied successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to apply gain: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Clear_Click(object? sender, EventArgs e)
        {
            audioFiles.Clear();
            UpdateFileGrid();
            if (gainSlider != null) gainSlider.Value = 0;
        }

        private void UpdateFileGrid()
        {
            if (fileGrid == null)
                return;

            fileGrid.Rows.Clear();
            foreach (var file in audioFiles)
            {
                string durationStr = file.Duration.TotalSeconds > 0
                    ? $"{file.Duration.Minutes:D2}:{file.Duration.Seconds:D2}"
                    : "—";

                fileGrid.Rows.Add(
                    file.FileName,
                    file.Format,
                    durationStr,
                    file.LoudnessIntegrated?.ToString("F2") ?? "—",
                    file.CalculatedGain?.ToString("+0.0;-0.0;0.0") ?? "—",
                    file.AppliedGain != 0 ? file.AppliedGain.ToString("+0.0;-0.0;0.0") : "—",
                    file.Status
                );
            }
        }
    }
}
