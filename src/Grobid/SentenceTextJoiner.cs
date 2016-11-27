using System.Text;

namespace Grobid.NET
{
    public class SentenceTextJoiner : IFeatureRowStringJoiner
    {
        public string Join(FeatureRow[] featureRows)
        {
            var sb = new StringBuilder();
            var state = SentenceTextState.Space;

            foreach (var featureRow in featureRows)
            {
                if (sb.Length > 0 && this.AbutsWord(featureRow.Value))
                {
                    sb.Replace(' ', featureRow.Value[0], sb.Length - 1, 1);
                }
                else if (sb.Length > 0 && featureRow.Value == "-")
                {
                    sb.Replace(' ', featureRow.Value[0], sb.Length - 1, 1);
                    state = SentenceTextState.NoSpace;
                }
                else if (state == SentenceTextState.NoSpace)
                {
                    sb.Remove(sb.Length - 1, 1); // trim extraneous whitespace
                    sb.Append(featureRow.Value);
                    state = SentenceTextState.Space;
                }
                else
                {
                    sb.Append(featureRow.Value);
                }

                sb.Append(" ");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1); // trim extraneous whitespace
            }

            return sb.ToString();
        }

        private bool AbutsWord(string value)
        {
            switch (value)
            {
                case ".":
                case "?":
                case "!":
                case ",":
                    return true;
                default:
                    return false;
            }
        }
    }
}