import axios, { AxiosError, AxiosResponse } from "axios";
import { useNavigate } from "react-router-dom";
import { useMutation, useQuery, useQueryClient } from "react-query";
import Config from "../config";
import { Order } from "../types/order";
import Problem from "../types/problem";
import { OrderDetails } from "../types/orderDetails";

const useAddOrder = () => {
    const queryClient = useQueryClient();
    const nav = useNavigate();
    return useMutation<AxiosResponse, AxiosError<Problem>, Order>(
        (o) => axios.post(`${Config.baseOrderApiUrl}/order`, o),
        {
          onSuccess: (resp, order) => {
            queryClient.invalidateQueries(["order", order.orderId]);
            nav("/");
          },
        }
      );
}

const useFetchOrderDetails = () => {
  return useQuery<OrderDetails[], AxiosError>("orders",  () =>
    axios.get(`${Config.baseOrderApiUrl}/order`).then((resp) => resp.data)
  );
};



export { useAddOrder, useFetchOrderDetails };