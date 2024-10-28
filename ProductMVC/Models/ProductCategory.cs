﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ProductMVC.Models;

/// <summary>
/// High-level product categorization.
/// </summary>
public partial class ProductCategory
{
    /// <summary>
    /// Primary key for ProductCategory records.
    /// </summary>
    public int ProductCategoryID { get; set; }

    /// <summary>
    /// Product category identification number of immediate ancestor category. Foreign key to ProductCategory.ProductCategoryID.
    /// </summary>
    public int? ParentProductCategoryID { get; set; }

    /// <summary>
    /// Category description.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
    /// </summary>
    public Guid rowguid { get; set; }

    /// <summary>
    /// Date and time the record was last updated.
    /// </summary>
    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<ProductCategory> InverseParentProductCategory { get; set; } = new List<ProductCategory>();

    public virtual ProductCategory ParentProductCategory { get; set; }

    public virtual ICollection<Product> Product { get; set; } = new List<Product>();
}