using System.Linq;
using UtilityDAL.View;

namespace UtilityDAL.DemoApp
{
    public class TeaTimeDbControl : FileDbControl
    {
        public TeaTimeDbControl() : base("tea", path =>
                  {
                      using (var tf = TeaTime.TeaFile<Price>.OpenRead(path))
                      {
                          return tf.Items.ToList();
                      }
                  })
        {
        }
    }
}