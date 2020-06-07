namespace UtilityDAL.DemoApp
{
    public class LiteDbControl : View.LiteDbControl<DummyDbObject>
    {
        public LiteDbControl() : base(_ => ((DummyDbObject)_).Id)
        {
        }
    }
}