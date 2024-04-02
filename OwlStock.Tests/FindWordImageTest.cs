using Xunit;

namespace OwlStock.Tests
{
    public class FindWordImageTest
    {
        [Fact]
        public void FindStartIndexPossition()
        {
            string filePath = @"C:\Program Files\Library\images\image.jpg";
            int position = 0;
            
            for (int i = 0; i < filePath.Length; i++)
            {
                string word = "";

                for (int j = i; j < i + 6; j++)
                {
                    word += filePath[j];
                }

                if (word.Equals("images"))
                {
                    position = i;
                    break;
                }

            }

            Assert.True(position > 0);
        }
    }
}
