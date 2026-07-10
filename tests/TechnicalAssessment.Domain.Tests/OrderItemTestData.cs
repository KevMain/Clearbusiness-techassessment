using System;

namespace TechnicalAssessment.Domain.Tests;

internal static class OrderItemTestData
{
    public static string Header => "order_id,item_id,list_price,discount";

    public static string First => "76,1,549.99,0.05";
    public static string Second => "76,2,1680.99,0.10";
    public static string Third => "104,1,449.00,0.10";

    public static System.Collections.Generic.IEnumerable<object[]> ValidCases => new[] {
        new object[] { First, 76, 1, 549.99m, 0.05m },
        new object[] { Second, 76, 2, 1680.99m, 0.10m },
        new object[] { Third, 104, 1, 449.00m, 0.10m }
    };

    public static System.Collections.Generic.IEnumerable<object[]> NullCases => new[] {
        new object[] { Header },
        new object[] { "X,1,549.99,0.05" },
        new object[] { "1,2" }
    };

    public static TechnicalAssessment.Domain.OrderItemCsvParser CreateParser() => new TechnicalAssessment.Domain.OrderItemCsvParser();

    public static string ItemWithMissingOrder => "888,1,99.99,0.00";
}
