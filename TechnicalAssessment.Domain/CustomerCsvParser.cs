namespace TechnicalAssessment.Domain;

public class CustomerCsvParser
{
    public Customer? Parse(string csvLine)
    {
        // Basic validation to ensure the line is not null or empty and has enough parts
        if (string.IsNullOrWhiteSpace(csvLine))
            return null;

        // Split the CSV line into parts. Assuming the CSV is well-formed and does not contain commas within quoted strings.
        var parts = csvLine.Split(',');
        if (parts.Length < 9)
            return null;

        try
        {
            return new Customer
            {
                CustomerId = int.Parse(parts[0].Trim()),
                FirstName = parts[1].Trim(),
                LastName = parts[2].Trim(),
                Phone = string.IsNullOrEmpty(parts[3].Trim()) ? null : parts[3].Trim(), // This could be empty, so we handle that case
                Email = parts[4].Trim(),
                Street = parts[5].Trim(),
                City = parts[6].Trim(),
                State = parts[7].Trim(),
                ZipCode = parts[8].Trim()
            };
        }
        catch
        {
            //TODO: Should handle errors in a better way, maybe log them or throw a custom exception
            return null;
        }
    }
}
