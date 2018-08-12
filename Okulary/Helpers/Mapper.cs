namespace Okulary.Helpers
{
    public class Mapper
    {
        public string MapujDodatnie(decimal value)
        {
            if (value > 0)
                return "+" + value.ToString();
            return value.ToString();
        }
    }
}
