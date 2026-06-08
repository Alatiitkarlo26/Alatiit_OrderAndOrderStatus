using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GadgetStoreDataService;
using GadgetStoreModels;

namespace Alatiit_Activity.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly GadgetStoreJsonData _jsonDataService;

        public OrderStatusController()
        {
            _jsonDataService = new GadgetStoreJsonData();
        }

        // GET: api/orders/products
        [HttpGet("products")]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            var products = _jsonDataService.GetProducts();
            return Ok(products);
        }

        // GET: api/orders/history
        [HttpGet("history")]
        public ActionResult<IEnumerable<Transaction>> GetTransactionHistory()
        {
            var history = _jsonDataService.GetHistory();
            return Ok(history);
        }

        // POST: api/orders/calculate-tax
        [HttpPost("calculate-tax")]
        public ActionResult<decimal> CalculateTax([FromQuery] decimal subtotal)
        {
            decimal totalWithTax = subtotal * 1.12m;
            return Ok(totalWithTax);
        }

        // GET: api/orders/can-purchase/productId/qty
        [HttpGet("can-purchase/{productId:guid}/{qty:int}")]
        public ActionResult<bool> CheckPurchaseAvailability(Guid productId, int qty)
        {
            var product = _jsonDataService.GetProducts().FirstOrDefault(p => p.ProductId == productId);
            bool available = product != null && product.Stock >= qty;
            return Ok(available);
        }

        // POST: api/orders/process
        [HttpPost("process")]
        public IActionResult CreateOrder([FromQuery] Guid productId, [FromQuery] int qty)
        {
            var product = _jsonDataService.GetProducts().FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            if (product.Stock < qty)
            {
                return BadRequest($"Requested quantity exceeds available stock. Current stock: {product.Stock}");
            }

            decimal subtotal = product.Price * qty;
            decimal totalWithTax = subtotal * 1.12m;

            var newSale = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                ProductId = productId,
                ProductName = product.Name,
                Quantity = qty,
                TotalPrice = totalWithTax,
                TransactionDate = DateTime.Now
            };

            _jsonDataService.UpdateProductStock(productId, qty);
            _jsonDataService.AddTransaction(newSale);

            return Ok(new { message = "Order processed successfully", transactionDetails = newSale });
        }

        // PUT: api/orders/update
        [HttpPut("update")]
        public IActionResult UpdateOrder([FromBody] Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Transaction data payload is required.");
            }

            _jsonDataService.UpdateTransaction(transaction);
            return Ok("Transaction updated successfully.");
        }

 
        // PATCH: api/orders/update
        [HttpPatch("products/{id:guid}")]
        public IActionResult PatchProduct(Guid id, [FromBody] Product partialProduct)
        {
            if (partialProduct == null)
            {
                return BadRequest("Product update data is required.");
            }

            var productsList = _jsonDataService.GetProducts();
            var existingProduct = productsList.FirstOrDefault(p => p.ProductId == id);

            if (existingProduct == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            existingProduct.Name = !string.IsNullOrEmpty(partialProduct.Name) ? partialProduct.Name : existingProduct.Name;

            existingProduct.Price = partialProduct.Price > 0 ? partialProduct.Price : existingProduct.Price;
            existingProduct.Stock = partialProduct.Stock >= 0 ? partialProduct.Stock : existingProduct.Stock;

            _jsonDataService.UpdateProductStock(id, 0);

            return Ok(new { message = "Product patched successfully", updatedProductDetails = existingProduct });
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteProduct(Guid id)
        {
            var productsList = _jsonDataService.GetProducts();
            var existingProduct = productsList.FirstOrDefault(p => p.ProductId == id);

            if (existingProduct == null)
            {
                return NotFound();
            }


            productsList.Remove(existingProduct);
            _jsonDataService.UpdateProductStock(id, existingProduct.Stock);

            return NoContent();
        }
    }
}