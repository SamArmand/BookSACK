namespace MysteriousDataProduct.Models 
{
    public class Word {
        public string WordString {get; set;}

        public string Subgenre {get; set;}

        public int FrequencyPlus1 {get; set;} = 1;

        public double Probability {get; set;}
    }
}

