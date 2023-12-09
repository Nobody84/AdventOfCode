namespace AOC2023.Puzzels.Day9
{
    public class Dataset
    {
        public Dataset(IEnumerable<int> history)
        {
            this.History = history.ToList();
        }

        public List<int> History { get; }

        public int GetPredicatedNumber(bool backward)
        {
            var rows = new List<List<int>>() { this.History };
            do
            {
                var currentRow = rows.Last();
                rows.Add(GetNextRow(currentRow));
            } while (!rows.Last().All(n => n == 0));

            if (backward)
            {
                this.FillupRowsIntoThePast(ref rows);
                return rows[0].First();
            }
            else
            {
                this.FillupRowsIntoTheFuture(ref rows);
                return rows[0].Last();
            }
        }

        private List<int> GetNextRow(List<int> row)
        {
            var nextRow = new List<int>();
            for (int i = 0; i < row.Count - 1; i++)
            {
                nextRow.Add(row[i + 1] - row[i]);
            }

            return nextRow;
        }

        private void FillupRowsIntoTheFuture(ref List<List<int>> rows)
        {
            for (int i = rows.Count - 1; i > 0; i--)
            {
                rows[i - 1].Add(rows[i].Last() + rows[i - 1].Last());
            }
        }

        private void FillupRowsIntoThePast(ref List<List<int>> rows)
        {
            for (int i = rows.Count - 1; i > 0; i--)
            {
                rows[i - 1].Insert(0, (rows[i - 1].First() - rows[i].First()));
            }
        }
    }
}