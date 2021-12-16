namespace WormsWorld.Model
{
    public class NameGenerator
    {
        private string _name = "Worm";
        private int _playersCount = 1;

        public string GenerateName(string name = "", int num = 0)
        {
            if (name == "")
            {
                string new_name = $"{_name}#{_playersCount}";
                _playersCount++;
                return new_name;
            }
            return $"{name}#{num}";
        }
    }
}