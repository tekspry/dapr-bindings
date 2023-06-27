import React, { useRef, useState, useEffect  } from "react";
import { Product } from "../types/product";
import { useGenerateProductDescription } from "../hooks/ProductHooks";


type Args = {
    product: Product;
    submitted: (product: Product) => void;  
};

const ProductForm = ({ product, submitted }: Args) => {
  const [productState, setProductState] = useState({ ...product });  
  const [loading, setLoading] = useState(false);
  const addProductDescription = useGenerateProductDescription();
  const textareaRef = useRef<HTMLTextAreaElement>(null);

  useEffect(() => {
    // Automatically adjust the height of the textarea on initial render
    adjustTextareaHeight();
  }, []);

  useEffect(() => {
    // Automatically adjust the height of the textarea when product description changes
    adjustTextareaHeight();
  }, [productState.productDescription]);
  

  const onSubmit: React.MouseEventHandler<HTMLButtonElement> = async (e) => {
    e.preventDefault();
    submitted(productState);
  };

  const onGenerateProductDescription: React.MouseEventHandler<HTMLButtonElement> = async (e) => {
    e.preventDefault();
    setLoading(true);
    addProductDescription.mutateAsync(productState, {
      onSuccess: (data) => {
        setLoading(false);
        setProductState((prevState)=> ({
          ...prevState,
          productDescription: data.data.productDescription.trim('//n//n'),
        }));
      },
    });
    
  };



  const onFileSelected = async (
    e: React.ChangeEvent<HTMLInputElement>
  ): Promise<void> => {
    e.preventDefault();
    e.target.files &&
      e.target.files[0] &&
      setProductState({
        ...productState,        
        imageUrl: await e.target.files[0].name,
      });
  };

  const adjustTextareaHeight = () => {
    if (textareaRef.current) {
      textareaRef.current.style.height = "auto";
      textareaRef.current.style.height = `${textareaRef.current.scrollHeight}px`;
    }
  };

  return (
    <form className="mt-2">
      <div className="form-group">
        <label htmlFor="name">Product Name</label>
        <input
          type="text"
          className="form-control"
          placeholder="Product Name"
          value={productState.name}
          onChange={(e) =>
            setProductState({ ...productState, name: e.target.value })
          }
        />
      </div>
      <div className="form-group mt-2">
        <label htmlFor="seller">Seller</label>
        <input
          type="text"
          className="form-control"
          placeholder="Seller"
          value={productState.seller}
          onChange={(e) =>
            setProductState({ ...productState, seller: e.target.value })
          }
        />
      </div>
      <div className="form-group mt-2">
        <label htmlFor="description">Short Description</label>
        <textarea
          className="form-control"
          placeholder="Description"
          value={productState.shortDescription}
          onChange={(e) =>
            setProductState({ ...productState, shortDescription: e.target.value })
          }
        />
      </div>
      <div className="form-group mt-2">
        <label htmlFor="price">Price</label>
        <input
          type="number"
          className="form-control"
          placeholder="Price"
          value={productState.price}
          onChange={(e) =>
            setProductState({ ...productState, price: parseInt(e.target.value) })
          }
        />
        </div>
        <div className="form-group mt-2">
        <label htmlFor="Quantity">Quantity</label>
        <input
          type="number"
          className="form-control"
          placeholder="Quantity"
          value={productState.quantity}
          onChange={(e) =>
            setProductState({ ...productState, quantity: parseInt(e.target.value) })
          }
        />
      </div>
      <div className="form-group mt-2">
        <label htmlFor="image">Image</label>
        <input
          id="image"
          type="file"
          className="form-control"
          onChange={onFileSelected}
        />
      </div>
      <div className="form-group mt-2">
        <label htmlFor="description">Product Description</label>
        <textarea
          ref={textareaRef}
          className="form-control"
          placeholder="Description"
          value={productState.productDescription}
          onChange={(e) =>
            setProductState({ ...productState, productDescription: e.target.value })
          }
        />
         <button
            className="btn btn-primary mt-2"
            disabled={!productState.name || !productState.price || loading}
            onClick={onGenerateProductDescription}
        >
        {loading ? "Generating..." : "Generate Product Description"}
      </button>
      </div>
      
      <button
        className="btn btn-primary mt-2"
        disabled={!productState.name || !productState.price}
        onClick={onSubmit}
      >
        Submit
      </button>
      {loading && (
        <div className="overlay">
          <div className="loader"></div>
        </div>
      )}
    </form>
  );
};

export default ProductForm;
