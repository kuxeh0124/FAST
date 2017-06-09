
namespace CoffeeBeanForm
{
    class TestResult
    {
        private string testName;

        public string tName
        {
            get { return testName; }
            set { testName = value; }
        }

        private string testResult;

        public string tResult
        {
            get { return tResult; }
            set { tResult = value; }
        }
        
        private string tResultLink;

        public string testResultLink
        {
            get { return tResultLink; }
            set { tResultLink = value; }
        }

        public TestResult(string testN, string testR, string testRL)
        {
            this.testName = testN;
            this.testResult = testR;
            this.testResultLink = testRL;            
        }
    }


}
