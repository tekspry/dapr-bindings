import { Link, useParams } from "react-router-dom";
import { useDeleteProduct, useFetchProduct } from "../hooks/ProductHooks";
import ApiStatus from "../apiStatus";
import { currencyFormatter } from "../config";
import defaultImage from "../main/logo.png";
import Orders from "../order/orders";


const ProductDetail = () => {
  const { id } = useParams();
  if (!id) throw Error("Product id not found");

  const { data, status, isSuccess } = useFetchProduct(id);

  const deleteProductMutation = useDeleteProduct();

  if (!isSuccess) return <ApiStatus status={status} />;

  if (!data) return <div>Product not found.</div>;

  return (
    <div className="row">
      <div className="col-6">
        <div className="row">
          <img
            className="img-fluid"
            src={"http://localhost:3000/" + data.imageUrl ? "http://localhost:3000/" + data.imageUrl : "http://localhost:3000/" + defaultImage}
            alt="Product pic"
          />
        </div>      
      </div>
      <div className="col-6">
        <div className="row mt-2">
          <h5 className="col-12">{data.name}</h5>
        </div>
        <div className="row">
          <h3 className="col-12">{data.seller}</h3>
        </div>
        <div className="row">
          <h2 className="themeFontColor col-12">
            {currencyFormatter.format(data.price)}
          </h2>
        </div>
        <div className="row">
          <div className="col-12 mt-3">{data.productDescription}</div>
        </div>  
        <Orders product={data} />    
      </div>
    </div>
  );
};

export default ProductDetail;
