using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantBusiness;
using RestaurantData;
using RestaurantDTOs;

namespace RestaurantApi.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpPost("AddNewProduct", Name = "AddNewProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsProductDTO> AddNewProduct(clsProductDTO ProductDTO)
        {
            try
            {
                clsProduct Product = new clsProduct();

                Product.ProductName = ProductDTO.ProductName;
                Product.ProductDescription = ProductDTO.ProductDescription;
                Product.Price = ProductDTO.Price;
                Product.CategoryID = ProductDTO.CategoryID;
                Product.ImageUrl = ProductDTO.ImageUrl;
                Product.Calories = ProductDTO.Calories;

                if (Product.Save())
                {
                    ProductDTO.ProductID = Product.ProductID;
                    return CreatedAtRoute("GetProductByID", new { ProductID = ProductDTO.ProductID }, ProductDTO);
                }
                else
                {
                    return BadRequest("The Product Could Not Be Added");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPut("UpdateProduct", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsProductDTO> UpdateUserRole(clsProductDTO ProductDTO)
        {
            try
            {
                clsProduct Product = clsProduct.Find(ProductDTO.ProductID);

                if (Product == null)
                {
                    return NotFound("Could Not Find The Product");
                }

                Product.ProductName = ProductDTO.ProductName;
                Product.ProductDescription = ProductDTO.ProductDescription;
                Product.Price = ProductDTO.Price;
                Product.CategoryID = ProductDTO.CategoryID;
                Product.ImageUrl = ProductDTO.ImageUrl;
                Product.Calories = ProductDTO.Calories;

                if (Product.Save())
                {
                    return Ok(Product.ProductDTO);
                }
                else
                {
                    return BadRequest("The Product Could Not Be Updated");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpDelete("DeleteProductByID/{ProductID}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult DeleteProduct(int ProductID)
        {
            try
            {
                clsProduct Product = clsProduct.Find(ProductID);

                if (Product == null)
                {
                    return NotFound("Could Not Find The Product");
                }

                if (clsProduct.DeleteProduct(ProductID))
                {
                    return Ok("The Product Deleted Successfully");
                }
                else
                {
                    return BadRequest("The Product Could Not Be Deleted");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetProductByID/{ProductID}", Name = "GetProductByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<clsProductDTO> GetProductByID(int ProductID)
        {
            try
            {
                clsProduct Product = clsProduct.Find(ProductID);

                if (Product == null)
                {
                    return NotFound("Could Not Find The Product");
                }
                else
                {
                    return Ok(Product.ProductDTO);
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetAllProducts", Name = "GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsProductDTO>> GetAllProducts()
        {
            try
            {
                List<clsProductDTO> ProductsList = clsProduct.GetAllProducts();

                if (ProductsList.Count == 0)
                {
                    return NotFound("There Is No Products To Show");
                }
                return Ok(ProductsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetAllProductsByIDs", Name = "GetAllProductsByIDs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsProductDTO>> GetAllProductsByIDs([FromQuery] List<int> ProductIDs)
        {
            try
            {
                List<clsProductDTO> ProductsList = clsProduct.GetAllProductsByIDs(ProductIDs);

                if (ProductsList.Count == 0)
                {
                    return NotFound("There Is No Products To Show");
                }
                return Ok(ProductsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpGet("GetAllProductsWithAddOn/{AddOnID}", Name = "GetAllProductsWithAddOn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsProductDTO>> GetAllProductsWithAddOn(int AddOnID)
        {
            try
            {
                List<clsProductDTO> ProductsList = clsProduct.GetAllProductsWithAddOn(AddOnID);

                if (ProductsList.Count == 0)
                {
                    return NotFound("There Is No Products To Show");
                }
                return Ok(ProductsList);
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpPost("AddProductAddOns", Name = "AddProductAddOns")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult<IEnumerable<clsAddOnDTO>> AddProductAddOns(clsProductAddOnsRequestDTO ProductAddOnsRequestDTO)
        {
            try
            {
                clsProduct Product = clsProduct.Find(ProductAddOnsRequestDTO.ProductID);

                if (Product == null)
                {
                    return NotFound("Could Not Find The Product");
                }
                if (Product.AddProductAddOns(ProductAddOnsRequestDTO.AddOnIDs))
                {
                    List<clsAddOnDTO> AddOnsList = clsAddOn.GetAddOnsByIDs(ProductAddOnsRequestDTO.AddOnIDs);
                    List<int> AddOnsIDs = AddOnsList.Select(addOn => addOn.AddOnID).ToList();

                    return CreatedAtRoute("GetAddOnsByIDs", new { AddOnIDs = AddOnsIDs }, AddOnsList);
                }
                else
                {
                    return BadRequest("The AddOns For The Product Could Not Be Added");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
        [HttpDelete("DeleteProductAddOns", Name = "DeleteProductAddOns")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public ActionResult DeleteProductAddOns([FromQuery] List<int> AddOnIDs,int ProductID)
        {
            try
            {
                clsProduct Product = clsProduct.Find(ProductID);

                if (Product == null)
                {
                    return NotFound("Could Not Find The Product");
                }

                if (Product.DeleteProductAddOns(AddOnIDs))
                {
                    return Ok("The AddOns For The Product Deleted Successfully");
                }
                else
                {
                    return BadRequest("The AddOns For The Product Could Not Be Deleted");
                }
            }
            catch (Exception ex)
            {
                return (ActionResult)clsAppGlobals.HandleError(ex);
            }
        }
    }
}
