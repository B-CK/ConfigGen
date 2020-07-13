using Tool.Config;
using Tool.Wrap;

namespace Tool.Check
{
    public class MacroChecker : Checker
    {
        public MacroChecker(FieldWrap define, string rule) : base(define, rule)
        {
        }

        public override void OutputError(Data data)
        {
            throw new System.NotImplementedException("暂时不支持该功能");
        }

        public override bool VerifyData(Data data)
        {
            throw new System.NotImplementedException("暂时不支持该功能");
        }

        public override bool VerifyRule()
        {
            throw new System.NotImplementedException("暂时不支持该功能");
        }
    }
}
