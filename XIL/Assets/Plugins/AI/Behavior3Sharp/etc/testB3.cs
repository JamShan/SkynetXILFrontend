namespace XIL.AI.Behavior3Sharp
{

    public class TestB3
    {

        public static void Test()
        {
            var tree = B3Functions.BuildBehavior3TreeFromConfig("etc/test_tree.json");
            tree.inspect();
            var blackboard = new Blackboard();
            for(int i=0; i< 1000; i++)
            {
                tree.Tick(i, blackboard);
            }
        }

    }


}