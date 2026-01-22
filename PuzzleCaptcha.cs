using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class PuzzlePiece
    {
        public int Index { get; set; }
        public Image Image { get; set; }
        public Point CorrectPosition { get; set; }
        public Point CurrentPosition { get; set; }
    }

    public class PuzzleCaptcha
    {
        private List<PuzzlePiece> pieces = new List<PuzzlePiece>();
        private Random random = new Random();
        private int pieceSize = 100; // Размер одной части пазла

        /// <summary>
        /// Разрезать изображение на 4 части (2x2)
        /// </summary>
        public List<PuzzlePiece> CreatePuzzle(Image sourceImage, int width, int height)
        {
            pieces.Clear();
            pieceSize = Math.Min(width / 2, height / 2);

            // Разрезаем изображение на 4 части
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 2; col++)
                {
                    Bitmap pieceBitmap = new Bitmap(pieceSize, pieceSize);
                    using (Graphics g = Graphics.FromImage(pieceBitmap))
                    {
                        Rectangle sourceRect = new Rectangle(
                            col * pieceSize,
                            row * pieceSize,
                            pieceSize,
                            pieceSize
                        );
                        Rectangle destRect = new Rectangle(0, 0, pieceSize, pieceSize);
                        g.DrawImage(sourceImage, destRect, sourceRect, GraphicsUnit.Pixel);
                    }

                    PuzzlePiece piece = new PuzzlePiece
                    {
                        Index = row * 2 + col,
                        Image = pieceBitmap,
                        CorrectPosition = new Point(col, row),
                        CurrentPosition = new Point(col, row)
                    };

                    pieces.Add(piece);
                }
            }

            return pieces;
        }

        /// <summary>
        /// Перемешать части пазла
        /// </summary>
        public void ShufflePieces()
        {
            // Перемешиваем позиции
            List<int> positions = new List<int> { 0, 1, 2, 3 };
            positions = positions.OrderBy(x => random.Next()).ToList();

            for (int i = 0; i < pieces.Count; i++)
            {
                int col = positions[i] % 2;
                int row = positions[i] / 2;
                pieces[i].CurrentPosition = new Point(col, row);
            }
        }

        /// <summary>
        /// Проверить, правильно ли собран пазл
        /// </summary>
        public bool IsPuzzleSolved()
        {
            foreach (var piece in pieces)
            {
                if (piece.CurrentPosition != piece.CorrectPosition)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Получить часть пазла по индексу
        /// </summary>
        public PuzzlePiece GetPiece(int index)
        {
            return pieces.FirstOrDefault(p => p.Index == index);
        }

        /// <summary>
        /// Получить все части пазла
        /// </summary>
        public List<PuzzlePiece> GetPieces()
        {
            return pieces;
        }

        public int GetPieceSize()
        {
            return pieceSize;
        }
    }
}
