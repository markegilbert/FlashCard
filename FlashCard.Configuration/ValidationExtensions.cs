using System.ComponentModel.DataAnnotations;


namespace FlashCard.Configuration
{
    public static class ValidationExtensions
    {

        // Adapted from https://stackoverflow.com/questions/17138749/how-to-manually-validate-a-model-with-attributes
        public static T ValidateDataAnnotations<T>(this T ObjectToValidate)
        {
            var context = new ValidationContext(ObjectToValidate);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(ObjectToValidate, context, validationResults);

            if (!isValid)
            {
                String ValidationResultsAsString = String.Join(" | ", (from v in validationResults select v.ErrorMessage).ToArray());
                throw new Exception($"Object was not valid: {ValidationResultsAsString}");
            }

            return ObjectToValidate;
        }
    }
}
