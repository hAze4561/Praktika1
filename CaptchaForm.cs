using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class CaptchaForm : Form
    {
        private PuzzleCaptcha puzzleCaptcha;
        private PictureBox[] puzzlePieces = new PictureBox[4]; // Части пазла (правая сетка)
        private PictureBox[] targetSlots = new PictureBox[4]; // Целевые области (левая сетка)
        private PictureBox draggedPiece = null;
        private Point dragOffset = Point.Empty;
        private int targetGridStartX = 80;  // Левая сетка (целевые области)
        private int targetGridStartY = 130;
        private int puzzleGridStartX = 280; // Правая сетка (части пазла)
        private int puzzleGridStartY = 130;
        private int puzzlePieceSize = 100;
        private bool isCaptchaSolved = false;

        public bool IsCaptchaSolved
        {
            get { return isCaptchaSolved; }
        }

        public CaptchaForm()
        {
            InitializeComponent();
        }

        private void CaptchaForm_Load(object sender, EventArgs e)
        {
            InitializePuzzle();
        }

        private void InitializePuzzle()
        {
            // Загружаем изображение для пазла
            Image puzzleImage = null;
            
            try
            {
                // ВАРИАНТ 1: Загрузить из Resources (добавьте изображение кота в Resources через Visual Studio)
                // 1. Откройте Properties -> Resources.resx
                // 2. Добавьте изображение кота
                // 3. Назовите его "cat" или "puzzle"
                // 4. Раскомментируйте строку ниже:
                // puzzleImage = Properties.Resources.cat;
                
                // ВАРИАНТ 2: Загрузить из файла (сохраните изображение кота в папку Resources)
                // Укажите полный путь к файлу:
                string imagePath = System.IO.Path.Combine(Application.StartupPath, "Resources", "cat.png");
                if (System.IO.File.Exists(imagePath))
                {
                    puzzleImage = Image.FromFile(imagePath);
                }
                else
                {
                    // Если файл не найден, используем тестовое изображение
                    puzzleImage = CreateTestImage();
                }
            }
            catch (Exception ex)
            {
                // Если не удалось загрузить, используем тестовое изображение
                puzzleImage = CreateTestImage();
            }

            // Создаем пазл
            puzzleCaptcha = new PuzzleCaptcha();
            puzzleCaptcha.CreatePuzzle(puzzleImage, 200, 200);
            puzzleCaptcha.ShufflePieces();

            // Создаем целевые области (левая сетка - серые квадраты)
            CreateTargetSlots();
            
            // Создаем части пазла (правая сетка - белые квадраты с изображениями)
            CreatePuzzleControls();
        }

        private Image CreateTestImage()
        {
            // Создаем простое тестовое изображение 200x200
            // ВАМ НУЖНО ЗАМЕНИТЬ ЭТО НА ЗАГРУЗКУ ВАШЕГО ИЗОБРАЖЕНИЯ КОТА
            Bitmap bmp = new Bitmap(200, 200);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                // Простой рисунок кота для теста
                g.FillEllipse(Brushes.LightGray, 50, 30, 100, 80); // Голова
                g.FillEllipse(Brushes.Tan, 60, 40, 20, 20); // Левое ухо
                g.FillEllipse(Brushes.Tan, 120, 40, 20, 20); // Правое ухо
                g.FillEllipse(Brushes.Black, 80, 60, 10, 10); // Левый глаз
                g.FillEllipse(Brushes.Black, 110, 60, 10, 10); // Правый глаз
                g.FillEllipse(Brushes.Pink, 95, 75, 10, 8); // Нос
            }
            return bmp;
        }

        private void CreateTargetSlots()
        {
            // Создаем 4 целевые области (левая сетка - серые квадраты)
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    int index = row * 2 + col;
                    PictureBox slot = new PictureBox
                    {
                        Size = new Size(puzzlePieceSize, puzzlePieceSize),
                        Location = new Point(
                            targetGridStartX + col * puzzlePieceSize,
                            targetGridStartY + row * puzzlePieceSize
                        ),
                        BackColor = Color.LightGray,
                        BorderStyle = BorderStyle.FixedSingle,
                        Tag = index,
                        SizeMode = PictureBoxSizeMode.StretchImage
                    };
                    
                    targetSlots[index] = slot;
                    this.Controls.Add(slot);
                    slot.SendToBack();
                }
            }
        }

        private void CreatePuzzleControls()
        {
            var pieces = puzzleCaptcha.GetPieces();
            puzzlePieceSize = puzzleCaptcha.GetPieceSize();

            // Размещаем части пазла в правой сетке (перемешанные)
            // Используем координаты 10+ для правой сетки, чтобы не пересекались с целевыми (0,0 до 1,1)
            for (int i = 0; i < pieces.Count; i++)
            {
                var piece = pieces[i];
                // Устанавливаем координаты в правой сетке (10+ для отличия от целевых)
                int col = piece.CurrentPosition.X;
                int row = piece.CurrentPosition.Y;
                piece.CurrentPosition = new Point(col + 10, row + 10);
                
                PictureBox pb = new PictureBox
                {
                    Size = new Size(puzzlePieceSize, puzzlePieceSize),
                    Location = new Point(
                        puzzleGridStartX + col * puzzlePieceSize,
                        puzzleGridStartY + row * puzzlePieceSize
                    ),
                    Image = piece.Image,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = Color.White,
                    Tag = piece.Index,
                    Cursor = Cursors.Hand
                };

                pb.MouseDown += PuzzlePiece_MouseDown;
                pb.MouseMove += PuzzlePiece_MouseMove;
                pb.MouseUp += PuzzlePiece_MouseUp;

                puzzlePieces[i] = pb;
                this.Controls.Add(pb);
                pb.BringToFront();
            }
        }

        private void PuzzlePiece_MouseDown(object sender, MouseEventArgs e)
        {
            draggedPiece = sender as PictureBox;
            dragOffset = new Point(e.X, e.Y);
            draggedPiece.BringToFront();
        }

        private void PuzzlePiece_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggedPiece != null)
            {
                Point newLocation = this.PointToClient(
                    Control.MousePosition
                );
                newLocation.Offset(-dragOffset.X, -dragOffset.Y);
                draggedPiece.Location = newLocation;
            }
        }

        private void PuzzlePiece_MouseUp(object sender, MouseEventArgs e)
        {
            if (draggedPiece != null)
            {
                int pieceIndex = (int)draggedPiece.Tag;
                var draggedPieceData = puzzleCaptcha.GetPiece(pieceIndex);
                
                // Проверяем, попала ли часть в целевую область (левая сетка)
                Point mousePos = this.PointToClient(Control.MousePosition);
                bool droppedInTarget = false;
                
                for (int i = 0; i < targetSlots.Length; i++)
                {
                    if (targetSlots[i].Bounds.Contains(mousePos))
                    {
                        // Часть попала в целевую область - размещаем её там
                        int targetCol = i % 2;
                        int targetRow = i / 2;
                        Point targetPosition = new Point(targetCol, targetRow);
                        
                        // Размещаем часть в целевой области
                        draggedPieceData.CurrentPosition = targetPosition;
                        draggedPiece.Location = new Point(
                            targetGridStartX + targetCol * puzzlePieceSize,
                            targetGridStartY + targetRow * puzzlePieceSize
                        );
                        droppedInTarget = true;
                        
                        // Проверяем, все ли части размещены и правильно ли
                        CheckAllPiecesPlaced();
                        break;
                    }
                }
                
                // Если не попала в целевую область, возвращаем в правую сетку
                if (!droppedInTarget)
                {
                    // Возвращаем в правую сетку (находим свободную позицию)
                    var pieces = puzzleCaptcha.GetPieces();
                    bool found = false;
                    for (int row = 0; row < 2 && !found; row++)
                    {
                        for (int col = 0; col < 2 && !found; col++)
                        {
                            Point testPos = new Point(col + 10, row + 10); // Координаты правой сетки
                            bool occupied = pieces.Any(p => p.Index != pieceIndex && p.CurrentPosition == testPos);
                            if (!occupied)
                            {
                                draggedPieceData.CurrentPosition = testPos;
                                draggedPiece.Location = new Point(
                                    puzzleGridStartX + col * puzzlePieceSize,
                                    puzzleGridStartY + row * puzzlePieceSize
                                );
                                found = true;
                            }
                        }
                    }
                    if (!found)
                    {
                        RefreshPuzzleDisplay();
                    }
                }

                draggedPiece = null;
            }
        }

        private void CheckAllPiecesPlaced()
        {
            // Проверяем, все ли части размещены в целевых областях (координаты 0,0 до 1,1)
            var pieces = puzzleCaptcha.GetPieces();
            bool allPlaced = true;
            
            foreach (var piece in pieces)
            {
                // Проверяем, находится ли часть в целевой области (левая сетка)
                // Целевые области имеют координаты от (0,0) до (1,1)
                if (piece.CurrentPosition.X < 0 || piece.CurrentPosition.X > 1 ||
                    piece.CurrentPosition.Y < 0 || piece.CurrentPosition.Y > 1)
                {
                    allPlaced = false;
                    break;
                }
            }
            
            // Если все части размещены в целевых областях
            if (allPlaced)
            {
                // Проверяем, правильно ли они размещены
                if (puzzleCaptcha.IsPuzzleSolved())
                {
                    // Все части размещены правильно - активируем кнопку OK
                    isCaptchaSolved = true;
                    btnOK.Enabled = true;
                    btnOK.BackColor = Color.FromArgb(0, 122, 204); // Синий цвет
                }
                else
                {
                    // Все части размещены, но неправильно
                    MessageBox.Show("Вы не прошли капчу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Перемешиваем пазл заново
                    ResetPuzzle();
                }
            }
            else
            {
                // Не все части размещены - деактивируем кнопку OK
                isCaptchaSolved = false;
                btnOK.Enabled = false;
                btnOK.BackColor = Color.Gray;
            }
        }

        private void ResetPuzzle()
        {
            puzzleCaptcha.ShufflePieces();
            // Устанавливаем все части обратно в правую сетку
            var pieces = puzzleCaptcha.GetPieces();
            for (int i = 0; i < pieces.Count; i++)
            {
                var piece = pieces[i];
                int col = piece.CurrentPosition.X;
                int row = piece.CurrentPosition.Y;
                piece.CurrentPosition = new Point(col + 10, row + 10);
            }
            RefreshPuzzleDisplay();
            isCaptchaSolved = false;
            btnOK.Enabled = false;
            btnOK.BackColor = Color.Gray;
        }

        private void RefreshPuzzleDisplay()
        {
            var pieces = puzzleCaptcha.GetPieces();
            for (int i = 0; i < puzzlePieces.Length; i++)
            {
                if (puzzlePieces[i] != null)
                {
                    var piece = pieces[i];
                    // Определяем, находится ли часть в целевой области (координаты 0,0 до 1,1) или в правой сетке (10+)
                    if (piece.CurrentPosition.X >= 0 && piece.CurrentPosition.X <= 1 &&
                        piece.CurrentPosition.Y >= 0 && piece.CurrentPosition.Y <= 1)
                    {
                        // Часть в целевой области (левая сетка)
                        puzzlePieces[i].Location = new Point(
                            targetGridStartX + piece.CurrentPosition.X * puzzlePieceSize,
                            targetGridStartY + piece.CurrentPosition.Y * puzzlePieceSize
                        );
                    }
                    else if (piece.CurrentPosition.X >= 10 && piece.CurrentPosition.Y >= 10)
                    {
                        // Часть в правой сетке - вычисляем позицию из CurrentPosition
                        int col = piece.CurrentPosition.X - 10;
                        int row = piece.CurrentPosition.Y - 10;
                        col = Math.Max(0, Math.Min(1, col));
                        row = Math.Max(0, Math.Min(1, row));
                        puzzlePieces[i].Location = new Point(
                            puzzleGridStartX + col * puzzlePieceSize,
                            puzzleGridStartY + row * puzzlePieceSize
                        );
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (puzzleCaptcha != null && puzzleCaptcha.IsPuzzleSolved())
            {
                // Пазл собран правильно - вход разрешен
                isCaptchaSolved = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // Проверяем, все ли части размещены
                var pieces = puzzleCaptcha.GetPieces();
                bool allPlaced = true;
                
                foreach (var piece in pieces)
                {
                    if (piece.CurrentPosition.X < 0 || piece.CurrentPosition.X > 1 ||
                        piece.CurrentPosition.Y < 0 || piece.CurrentPosition.Y > 1)
                    {
                        allPlaced = false;
                        break;
                    }
                }
                
                if (allPlaced)
                {
                    // Все части размещены, но неправильно
                    MessageBox.Show("Вы не прошли капчу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ResetPuzzle();
                }
                else
                {
                    MessageBox.Show("Соберите пазл полностью", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Перемешиваем пазл заново
            if (puzzleCaptcha != null)
            {
                ResetPuzzle();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
