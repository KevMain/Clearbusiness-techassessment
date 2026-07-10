using System;

namespace TechnicalAssessment.Domain.Tests;

internal static class CustomerTestData
{
    public static string Header => "customer_id,first_name,last_name,phone,email,street,city,state,zip_code";

    public static string Debra => "1,Debra,Burks,,debra.burks@yahoo.com,9273 Thorne Ave. ,Orchard Park,NY,14127";

    public static string Kasha => " 2 , Kasha , Todd , , kasha.todd@yahoo.com , 910 Vine Street  , Campbell , CA , 95008 ";

    public static string Charolette => "5,Charolette,Rice,(916) 381-6003,charolette.rice@msn.com,107 River Dr. ,Sacramento,CA,95820";

    public static string Invalid => "X,Invalid,User,,invalid@example.com,Some St,City,ST,12345";

    public static string TooFew => "1,Debra,Burks";

    public static TechnicalAssessment.Domain.CustomerCsvParser CreateParser() => new TechnicalAssessment.Domain.CustomerCsvParser();

    public static System.Collections.Generic.IEnumerable<object[]> ValidCases => new[] {
        new object[] { Debra, 1, "Debra", "Burks", (string?)null, "debra.burks@yahoo.com", "9273 Thorne Ave.", "Orchard Park", "NY", "14127" },
        new object[] { Kasha, 2, "Kasha", "Todd", (string?)null, "kasha.todd@yahoo.com", "910 Vine Street", "Campbell", "CA", "95008" },
        new object[] { Charolette, 5, "Charolette", "Rice", "(916) 381-6003", "charolette.rice@msn.com", "107 River Dr.", "Sacramento", "CA", "95820" }
    };

    public static System.Collections.Generic.IEnumerable<object[]> NullCases => new[] {
        new object[] { Header },
        new object[] { Invalid },
        new object[] { TooFew }
    };
}
