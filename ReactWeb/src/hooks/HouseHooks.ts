import { House } from "../types/house";
import config from "../config";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import axios, { AxiosError, AxiosResponse } from "axios";
import { useNavigate } from "react-router-dom";
import Problem from "../types/problem";

const useFetchHouses = () => {
    
    //#region 
    // const [houses, setHouses] = useState<House[]>([]);

    // useEffect(() => {

    //     const fetchHouses = async () => {
    //         const result = await fetch(`${config.baseApiUrl}/houses`);
    //         const houses = await result.json();
    //         setHouses(houses);
    //     };
    //     fetchHouses();

    // }, []);

    // return houses;
    //#endregion

    //this form is more effective, and react query takes care of the state and effect of the fecthing data.
    return useQuery<House[], AxiosError>({
        queryKey: ["houses"],
        queryFn: () => 
            axios.get(`${config.baseApiUrl}/houses`).then((resp) => resp.data)
    });
};

const useFetchHouse = (id: number) => {
    return useQuery<House, AxiosError>({
        queryKey: ["houses", id],
        queryFn: () => 
            axios.get(`${config.baseApiUrl}/house/${id}`)
            .then((resp) => resp.data)
    });
}

const useAddHouse = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();

    return useMutation<AxiosResponse, AxiosError<Problem>, House>({
        mutationFn: (h) => axios.post(`${config.baseApiUrl}/houses`, h),
        onSuccess: () => {
            queryClient.invalidateQueries({
                queryKey: ["houses"]
            });
            nav("/")
        }
    });
}

const useUpdateHouse = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();

    return useMutation<AxiosResponse, AxiosError<Problem>, House>({
        mutationFn: (h) => axios.put(`${config.baseApiUrl}/houses`, h),
        onSuccess: (_, house) => {
            queryClient.invalidateQueries({
                queryKey: ["houses"]
            });
            nav(`/house/${house.id}`);
        }
    });
}

const useDeleteHouse = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();

    return useMutation<AxiosResponse, AxiosError, House>({
        mutationFn: (h) => axios.delete(`${config.baseApiUrl}/house/${h.id}`),
        onSuccess: () => {
            queryClient.invalidateQueries({
                queryKey: ["houses"]
            });
            nav("/");
        }
    });
}

export default useFetchHouses;
export { useFetchHouse, useAddHouse, useUpdateHouse, useDeleteHouse };

