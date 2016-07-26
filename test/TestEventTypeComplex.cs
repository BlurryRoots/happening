namespace BlurryRoots.Happening.Test {

    using System.Collections.Generic;

    public class TestEventTypeComplex {

        public List<int> Dates {
            get; private set;
        }

        public TestEventTypeComplex (int a, int b) {
            this.Dates = new List<int> ();
            this.Dates.Add (a);
            this.Dates.Add (b);
        }

    }

}
