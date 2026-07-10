namespace TechnicalAssessment.Domain.Tests;

using System;

internal static class OrderTestData
{
    public static string Header => "order_id,customer_id,order_status,order_date,required_date,shipped_date";

    public static string FirstLine => "76,9,3,16/02/2016,16/02/2016,";
    public static string SecondLine => "104,7,4,03/03/2016,05/03/2016,05/03/2016";
    public static string ThirdLine => "108,12,4,06/03/2016,09/03/2016,07/03/2016";

    public static System.Collections.Generic.IEnumerable<object[]> ValidCases => new[] {
        new object[] { FirstLine, 76, 9, 3, new System.DateTime(2016,2,16), new System.DateTime(2016,2,16), (DateTime?)null },
        new object[] { SecondLine, 104, 7, 4, new System.DateTime(2016,3,3), new System.DateTime(2016,3,5), new System.DateTime(2016,3,5) },
        new object[] { ThirdLine, 108, 12, 4, new System.DateTime(2016,3,6), new System.DateTime(2016,3,9), new System.DateTime(2016,3,7) }
    };

    public static System.Collections.Generic.IEnumerable<object[]> NullCases => new[] {
        new object[] { Header },
        new object[] { "X,1,3,01/01/2016,01/01/2016," },
        new object[] { "1,2" }
    };

    public static string OrderWithMissingCustomer => "200,999,1,01/01/2016,01/01/2016,";

    public static TechnicalAssessment.Domain.OrderCsvParser CreateParser() => new TechnicalAssessment.Domain.OrderCsvParser();
}
