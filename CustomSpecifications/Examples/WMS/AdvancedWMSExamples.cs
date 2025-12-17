using CustomSpecifications.Core;
using CustomSpecifications.Examples.WMS.Models;
using CustomSpecifications.Examples.WMS.Specifications;
using CustomSpecifications.Extensions;

namespace CustomSpecifications.Examples.WMS;

/// <summary>
/// Advanced examples demonstrating real-world 3PL Warehouse Management System scenarios.
/// These examples showcase complex business rules and specification composition.
/// </summary>
public static class AdvancedWMSExamples
{
    /// <summary>
    /// Example 1: Identify inventory requiring reorder for premium clients.
    /// Business Rule: Notify premium/enterprise clients about low stock before standard clients.
    /// </summary>
    public static void Example1_LowStockPremiumClients()
    {
        Console.WriteLine("=== Example 1: Low Stock Alert for Premium Clients ===\n");

        var clients = GetSampleClients();
        var inventory = GetSampleInventory();

        // Specifications
        var isPremiumClient = new ClientSpecifications.IsPremiumOrEnterpriseSpecification();
        var isActive = new ClientSpecifications.IsActiveSpecification();
        var isBelowReorder = new InventorySpecifications.IsBelowReorderPointSpecification();
        var isAvailable = new InventorySpecifications.IsAvailableSpecification();

        // Complex rule: Premium/Enterprise clients with active contracts having low stock
        var premiumActiveClients = clients.Where(isPremiumClient.And(isActive)).ToList();
        var lowStockInventory = inventory.Where(isBelowReorder.Or(new InventorySpecifications.IsOutOfStockSpecification())).ToList();

        Console.WriteLine("Premium clients with low stock items:");
        foreach (var client in premiumActiveClients)
        {
            var clientLowStock = lowStockInventory.Where(inv => inv.ClientId == client.Id).ToList();
            if (clientLowStock.Any())
            {
                Console.WriteLine($"\n  Client: {client.Name} (Tier: {client.Tier})");
                foreach (var inv in clientLowStock)
                {
                    Console.WriteLine($"    - SKU: {inv.Sku}, Qty: {inv.Quantity}, Reorder Point: {inv.ReorderPoint}");
                }
            }
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Example 2: Identify orders requiring expedited processing.
    /// Business Rule: Rush orders, orders due within 8 hours, or overnight shipping need immediate attention.
    /// </summary>
    public static void Example2_ExpeditedOrderProcessing()
    {
        Console.WriteLine("=== Example 2: Expedited Order Processing ===\n");

        var orders = GetSampleOrders();

        // Complex specification combining multiple urgent criteria
        var requiresExpedited = new OrderSpecifications.RequiresExpeditedProcessingSpecification();
        var isPending = new OrderSpecifications.HasStatusSpecification(OrderStatus.Pending);

        var urgentPendingOrders = orders.Where(requiresExpedited.And(isPending)).ToList();

        Console.WriteLine($"Urgent pending orders requiring immediate processing ({urgentPendingOrders.Count}):\n");
        foreach (var order in urgentPendingOrders.OrderBy(o => o.RequiredDate))
        {
            var timeRemaining = order.RequiredDate - DateTime.UtcNow;
            Console.WriteLine($"  Order: {order.Id}");
            Console.WriteLine($"    Priority: {order.Priority}, Shipping: {order.ShippingMethod}");
            Console.WriteLine($"    Due: {order.RequiredDate:g} ({timeRemaining.TotalHours:F1} hours)");
            Console.WriteLine($"    Lines: {order.Lines.Count} items\n");
        }
    }

    /// <summary>
    /// Example 3: Warehouse location assignment for special handling products.
    /// Business Rule: Hazmat products must go to approved locations, refrigerated items to
    /// temperature-controlled zones, fragile items should avoid high shelves.
    /// </summary>
    public static void Example3_SpecialHandlingLocationAssignment()
    {
        Console.WriteLine("=== Example 3: Special Handling Location Assignment ===\n");

        var products = GetSampleProducts();
        var locations = GetSampleLocations();

        var requiresSpecialHandling = new ProductSpecifications.RequiresSpecialHandlingSpecification();
        var isHazmat = new ProductSpecifications.IsHazmatSpecification();
        var requiresRefrigeration = new ProductSpecifications.RequiresRefrigerationSpecification();

        var specialProducts = products.Where(requiresSpecialHandling).ToList();

        Console.WriteLine($"Products requiring special handling ({specialProducts.Count}):\n");

        foreach (var product in specialProducts)
        {
            Console.WriteLine($"  SKU: {product.Sku} - {product.Name}");
            Console.WriteLine($"    Attributes: " +
                             $"{(product.IsHazmat ? "Hazmat " : "")}" +
                             $"{(product.IsFragile ? "Fragile " : "")}" +
                             $"{(product.RequiresRefrigeration ? "Refrigerated" : "")}");

            // Find suitable locations
            var suitableLocations = locations.Where(loc =>
                (!product.IsHazmat || loc.IsHazmatApproved) &&
                (!product.RequiresRefrigeration || loc.IsTemperatureControlled) &&
                product.Weight <= loc.MaxWeight
            ).ToList();

            Console.WriteLine($"    Suitable locations: {suitableLocations.Count}");
            foreach (var loc in suitableLocations.Take(3))
            {
                Console.WriteLine($"      - {loc.Id} ({loc.Zone}-{loc.Aisle}-{loc.Bay}-{loc.Level})");
            }
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Example 4: Expiring inventory management with client notification.
    /// Business Rule: Identify products expiring within 30 days and categorize by urgency.
    /// </summary>
    public static void Example4_ExpiringInventoryManagement()
    {
        Console.WriteLine("=== Example 4: Expiring Inventory Management ===\n");

        var products = GetSampleProducts();
        var inventory = GetSampleInventory();

        var isExpiring30Days = new ProductSpecifications.IsExpiringSpecification(30);
        var isExpiring7Days = new ProductSpecifications.IsExpiringSpecification(7);
        var isExpired = new ProductSpecifications.IsExpiredSpecification();
        var isAvailable = new InventorySpecifications.IsAvailableSpecification();

        var expiringProducts = products.Where(isExpiring30Days).ToList();

        Console.WriteLine("Expiring Inventory Report:\n");

        // Critical: Expired
        var expiredProducts = products.Where(isExpired).ToList();
        Console.WriteLine($"CRITICAL - Expired ({expiredProducts.Count}):");
        foreach (var product in expiredProducts)
        {
            var inv = inventory.FirstOrDefault(i => i.Sku == product.Sku);
            if (inv != null && inv.Quantity > 0)
            {
                Console.WriteLine($"  ? {product.Sku} - Qty: {inv.Quantity}, Expired: {product.ExpirationDate:d}");
            }
        }

        // High: Expiring within 7 days
        var expiringSoon = products.Where(isExpiring7Days.AndNot(isExpired)).ToList();
        Console.WriteLine($"\nHIGH - Expiring within 7 days ({expiringSoon.Count}):");
        foreach (var product in expiringSoon)
        {
            var inv = inventory.FirstOrDefault(i => i.Sku == product.Sku && i.Quantity > 0);
            if (inv != null)
            {
                var daysRemaining = (product.ExpirationDate!.Value - DateTime.UtcNow).Days;
                Console.WriteLine($"  ! {product.Sku} - Qty: {inv.Quantity}, Days left: {daysRemaining}");
            }
        }

        // Medium: Expiring within 30 days but not within 7
        var expiringMedium = products.Where(isExpiring30Days.AndNot(isExpiring7Days)).ToList();
        Console.WriteLine($"\nMEDIUM - Expiring within 30 days ({expiringMedium.Count}):");
        foreach (var product in expiringMedium)
        {
            var inv = inventory.FirstOrDefault(i => i.Sku == product.Sku && i.Quantity > 0);
            if (inv != null)
            {
                var daysRemaining = (product.ExpirationDate!.Value - DateTime.UtcNow).Days;
                Console.WriteLine($"  • {product.Sku} - Qty: {inv.Quantity}, Days left: {daysRemaining}");
            }
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Example 5: Multi-client order batching restrictions.
    /// Business Rule: Cannot batch orders from different clients in same wave,
    /// but can batch multiple orders from same client with compatible shipping methods.
    /// </summary>
    public static void Example5_OrderBatchingLogic()
    {
        Console.WriteLine("=== Example 5: Order Batching Logic ===\n");

        var orders = GetSampleOrders();

        var isPending = new OrderSpecifications.HasStatusSpecification(OrderStatus.Pending);
        var isGroundShipping = new OrderSpecifications.HasShippingMethodSpecification(ShippingMethod.Ground);
        var notUrgent = new OrderSpecifications.IsUrgentSpecification().Not();

        // Eligible for batching: Pending + Ground shipping + Not urgent
        var batchableOrders = orders.Where(isPending.And(isGroundShipping).And(notUrgent)).ToList();

        Console.WriteLine($"Orders eligible for batching ({batchableOrders.Count}):\n");

        // Group by client for batch processing
        var batchesByClient = batchableOrders.GroupBy(o => o.ClientId);

        foreach (var clientBatch in batchesByClient)
        {
            var clientOrders = clientBatch.ToList();
            var totalLines = clientOrders.Sum(o => o.Lines.Count);

            Console.WriteLine($"  Client: {clientBatch.Key}");
            Console.WriteLine($"    Orders in batch: {clientOrders.Count}");
            Console.WriteLine($"    Total line items: {totalLines}");
            Console.WriteLine($"    Order IDs: {string.Join(", ", clientOrders.Select(o => o.Id))}");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Example 6: SLA compliance monitoring for shipments.
    /// Business Rule: Track shipments that are delayed or have long delivery times,
    /// grouped by client tier for priority resolution.
    /// </summary>
    public static void Example6_SLAComplianceMonitoring()
    {
        Console.WriteLine("=== Example 6: SLA Compliance Monitoring ===\n");

        var clients = GetSampleClients();
        var shipments = GetSampleShipments();

        var hasIssues = new ShipmentSpecifications.HasDeliveryIssuesSpecification();
        var isDelayed = new ShipmentSpecifications.IsDelayedSpecification();
        var isPremiumClient = new ClientSpecifications.IsPremiumOrEnterpriseSpecification();

        var problematicShipments = shipments.Where(hasIssues).ToList();

        Console.WriteLine($"Shipments with delivery issues ({problematicShipments.Count}):\n");

        // Prioritize by client tier
        foreach (var shipment in problematicShipments.OrderBy(s => s.Status))
        {
            var client = clients.FirstOrDefault(c => c.Id == shipment.ClientId);
            var isPremium = client != null && isPremiumClient.IsSatisfiedBy(client);
            var priority = isPremium ? "HIGH" : "NORMAL";

            Console.WriteLine($"  Shipment: {shipment.Id} [Priority: {priority}]");
            Console.WriteLine($"    Client: {client?.Name ?? shipment.ClientId} ({client?.Tier})");
            Console.WriteLine($"    Status: {shipment.Status}");
            Console.WriteLine($"    Carrier: {shipment.Carrier}");
            Console.WriteLine($"    Tracking: {shipment.TrackingNumber}");
            Console.WriteLine($"    Ship Date: {shipment.ShipDate:d}");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Example 7: Cycle count prioritization.
    /// Business Rule: Prioritize cycle counts for high-value items, items needing recount,
    /// and items for premium clients.
    /// </summary>
    public static void Example7_CycleCountPrioritization()
    {
        Console.WriteLine("=== Example 7: Cycle Count Prioritization ===\n");

        var inventory = GetSampleInventory();
        var products = GetSampleProducts();
        var clients = GetSampleClients();

        var needsCycleCount = new InventorySpecifications.NeedsCycleCountSpecification(30);
        var isAvailable = new InventorySpecifications.IsAvailableSpecification();

        var inventoryNeedingCount = inventory.Where(needsCycleCount.And(isAvailable)).ToList();

        Console.WriteLine($"Inventory items requiring cycle count ({inventoryNeedingCount.Count}):\n");

        // Prioritize by: 1) Premium clients, 2) High-value products, 3) Days since last count
        var prioritizedCounts = inventoryNeedingCount
            .Select(inv => new
            {
                Inventory = inv,
                Product = products.FirstOrDefault(p => p.Sku == inv.Sku),
                Client = clients.FirstOrDefault(c => c.Id == inv.ClientId),
                DaysSinceCount = (DateTime.UtcNow - inv.LastCountDate).Days
            })
            .OrderByDescending(x => x.Client?.Tier == ClientTier.Enterprise ? 3 :
                                    x.Client?.Tier == ClientTier.Premium ? 2 : 1)
            .ThenByDescending(x => x.Product?.UnitCost ?? 0)
            .ThenByDescending(x => x.DaysSinceCount)
            .Take(10);

        Console.WriteLine("Top 10 priority cycle counts:\n");
        int rank = 1;
        foreach (var item in prioritizedCounts)
        {
            Console.WriteLine($"  {rank}. SKU: {item.Inventory.Sku}");
            Console.WriteLine($"     Client: {item.Client?.Name ?? item.Inventory.ClientId} ({item.Client?.Tier})");
            Console.WriteLine($"     Value: ${item.Product?.UnitCost:F2}, Qty: {item.Inventory.Quantity}");
            Console.WriteLine($"     Days since count: {item.DaysSinceCount}");
            Console.WriteLine($"     Location: {item.Inventory.LocationId}");
            Console.WriteLine();
            rank++;
        }
    }

    /// <summary>
    /// Example 8: International shipment compliance check.
    /// Business Rule: Verify international orders have proper documentation and
    /// identify items that cannot be shipped internationally.
    /// </summary>
    public static void Example8_InternationalShipmentCompliance()
    {
        Console.WriteLine("=== Example 8: International Shipment Compliance ===\n");

        var orders = GetSampleOrders();
        var products = GetSampleProducts();

        var isInternational = new OrderSpecifications.IsInternationalSpecification();
        var isReadyToShip = new OrderSpecifications.IsReadyToShipSpecification();
        var isHazmat = new ProductSpecifications.IsHazmatSpecification();

        var internationalOrders = orders.Where(isInternational).ToList();

        Console.WriteLine($"International orders ({internationalOrders.Count}):\n");

        foreach (var order in internationalOrders)
        {
            Console.WriteLine($"  Order: {order.Id}");
            Console.WriteLine($"    Destination: {order.DestinationCountry}");
            Console.WriteLine($"    Status: {order.Status}");

            // Check for restricted items
            var orderProducts = order.Lines
                .Select(line => products.FirstOrDefault(p => p.Sku == line.Sku))
                .Where(p => p != null)
                .ToList();

            var hasHazmat = orderProducts.Any(p => p!.IsHazmat);
            var hasPerishable = orderProducts.Any(p => p!.ExpirationDate.HasValue);

            if (hasHazmat || hasPerishable)
            {
                Console.WriteLine($"    ? COMPLIANCE ISSUES:");
                if (hasHazmat)
                    Console.WriteLine($"      - Contains HAZMAT items (special documentation required)");
                if (hasPerishable)
                    Console.WriteLine($"      - Contains perishable items (expedited shipping required)");
            }
            else
            {
                Console.WriteLine($"    ? No compliance issues");
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Run all advanced WMS examples.
    /// </summary>
    public static void RunAll()
    {
        Example1_LowStockPremiumClients();
        Example2_ExpeditedOrderProcessing();
        Example3_SpecialHandlingLocationAssignment();
        Example4_ExpiringInventoryManagement();
        Example5_OrderBatchingLogic();
        Example6_SLAComplianceMonitoring();
        Example7_CycleCountPrioritization();
        Example8_InternationalShipmentCompliance();
    }

    #region Sample Data Generation

    private static List<Client> GetSampleClients() => new()
    {
        ClientFactory.Create("TR001", "Tire Rack", "logistics@tirerack.com", ClientTier.Enterprise,
            DateTime.UtcNow.AddYears(-2), DateTime.UtcNow.AddYears(1)),
        ClientFactory.Create("FB001", "Fenty Beauty", "warehouse@fentybeauty.com", ClientTier.Premium,
            DateTime.UtcNow.AddYears(-1), DateTime.UtcNow.AddMonths(6)),
        ClientFactory.Create("NE001", "Newegg", "fulfillment@newegg.com", ClientTier.Enterprise,
            DateTime.UtcNow.AddYears(-3), DateTime.UtcNow.AddYears(2)),
        ClientFactory.Create("SM001", "Small Retailer", "orders@small.com", ClientTier.Standard,
            DateTime.UtcNow.AddMonths(-6), DateTime.UtcNow.AddMonths(6))
    };

    private static List<Product> GetSampleProducts() => new()
    {
        ProductFactory.Create("TIRE-001", "TR001", "All-Season Tire 225/65R17", "Premium all-season tire",
            ProductCategory.Automotive, 25.0m, DimensionsFactory.Create(28, 9, 28), false, false, false, 120.00m),
        ProductFactory.Create("FB-LIP-001", "FB001", "Fenty Icon Lipstick", "Velvet liquid lipstick",
            ProductCategory.Beauty, 0.15m, DimensionsFactory.Create(4, 1, 1), true, false, false, 25.00m,
            DateTime.UtcNow.AddMonths(18)),
        ProductFactory.Create("NE-GPU-001", "NE001", "RTX 4090 Graphics Card", "High-end GPU",
            ProductCategory.Electronics, 5.0m, DimensionsFactory.Create(12, 5, 2), true, false, false, 1599.00m),
        ProductFactory.Create("CHEM-001", "TR001", "Tire Sealant Spray", "Emergency tire repair",
            ProductCategory.Automotive, 1.5m, DimensionsFactory.Create(10, 3, 3), false, true, false, 15.00m),
        ProductFactory.Create("FB-FOUND-001", "FB001", "Pro Filt'r Foundation", "Soft matte foundation",
            ProductCategory.Beauty, 0.3m, DimensionsFactory.Create(5, 2, 2), true, false, false, 40.00m,
            DateTime.UtcNow.AddDays(10))
    };

    private static List<Inventory> GetSampleInventory() => new()
    {
        InventoryFactory.Create("INV-001", "TIRE-001", "TR001", "LOC-A1", 45, 100, 500,
            DateTime.UtcNow.AddDays(-45), InventoryStatus.Available),
        InventoryFactory.Create("INV-002", "FB-LIP-001", "FB001", "LOC-B2", 250, 200, 1000,
            DateTime.UtcNow.AddDays(-15), InventoryStatus.Available),
        InventoryFactory.Create("INV-003", "NE-GPU-001", "NE001", "LOC-C3", 8, 10, 50,
            DateTime.UtcNow.AddDays(-20), InventoryStatus.Available),
        InventoryFactory.Create("INV-004", "CHEM-001", "TR001", "LOC-D4", 0, 50, 200,
            DateTime.UtcNow.AddDays(-60), InventoryStatus.Available),
        InventoryFactory.Create("INV-005", "FB-FOUND-001", "FB001", "LOC-B1", 150, 100, 800,
            DateTime.UtcNow.AddDays(-10), InventoryStatus.Available)
    };

    private static List<Location> GetSampleLocations() => new()
    {
        LocationFactory.Create("LOC-A1", "A", "01", "01", "1", LocationType.Storage, false, 5000, false),
        LocationFactory.Create("LOC-B1", "B", "02", "01", "1", LocationType.Storage, true, 2000, false),
        LocationFactory.Create("LOC-B2", "B", "02", "02", "2", LocationType.Storage, true, 2000, false),
        LocationFactory.Create("LOC-C3", "C", "03", "03", "1", LocationType.Storage, false, 1000, false),
        LocationFactory.Create("LOC-D4", "D", "04", "01", "1", LocationType.Storage, false, 3000, true)
    };

    private static List<Order> GetSampleOrders() => new()
    {
        OrderFactory.Create("ORD-001", "TR001", DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddHours(6),
            OrderPriority.Rush, ShippingMethod.Overnight, "USA", OrderStatus.Pending,
            new List<OrderLine> { OrderLineFactory.Create("TIRE-001", 4) }),
        OrderFactory.Create("ORD-002", "FB001", DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(3),
            OrderPriority.Normal, ShippingMethod.Ground, "USA", OrderStatus.Pending,
            new List<OrderLine> { OrderLineFactory.Create("FB-LIP-001", 50), OrderLineFactory.Create("FB-FOUND-001", 30) }),
        OrderFactory.Create("ORD-003", "NE001", DateTime.UtcNow, DateTime.UtcNow.AddDays(1),
            OrderPriority.High, ShippingMethod.TwoDayAir, "Canada", OrderStatus.Pending,
            new List<OrderLine> { OrderLineFactory.Create("NE-GPU-001", 2) })
    };

    private static List<Shipment> GetSampleShipments() => new()
    {
        ShipmentFactory.Create("SHIP-001", "ORD-100", "TR001", DateTime.UtcNow.AddDays(-5),
            "FedEx", "1Z999AA10123456784", 100.0m, ShipmentStatus.InTransit),
        ShipmentFactory.Create("SHIP-002", "ORD-101", "FB001", DateTime.UtcNow.AddDays(-3),
            "UPS", "1Z999AA10123456785", 5.0m, ShipmentStatus.Delayed),
        ShipmentFactory.Create("SHIP-003", "ORD-102", "NE001", DateTime.UtcNow.AddDays(-10),
            "USPS", "9400100000000000000000", 10.0m, ShipmentStatus.Delivered,
            DateTime.UtcNow.AddDays(-2))
    };

    #endregion
}
