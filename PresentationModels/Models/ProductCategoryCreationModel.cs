﻿using Domain.Aggregates.ProductAggregate;
using System.ComponentModel.DataAnnotations;

namespace PresentationModels.Models
{
    public class ProductCategoryCreationModel
    {
        [Required]
        [MinLength(ProductCategory.MIN_LENGTH)]
        [MaxLength(ProductCategory.MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        [MinLength(ProductCategory.MIN_LENGTH)]
        [MaxLength(ProductCategory.MAX_DESCRIPTION_LENGTH)]
        public string Description { get; set; }
    }
}
