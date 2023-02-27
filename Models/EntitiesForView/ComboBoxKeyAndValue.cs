namespace Firma.Models.EntitiesForView
{
    public class ComboBoxKeyAndValue
    {
        #region Konstruktory
        public ComboBoxKeyAndValue() {}
        public ComboBoxKeyAndValue(int key, string value)
        {
            Key= key;
            Value= value;
        }
        #endregion
        /// <summary>
        /// Pole do przekazania do Modelu. Bedzie to przykładowo IdKontrahenta
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Wartość wyświetlana użytkownikowi na widoku
        /// </summary>
        public string Value { get; set; }
    }
}
