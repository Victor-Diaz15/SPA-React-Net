import ValidationSummary from "../ValidationSummary";
import { useAddHouse } from "../hooks/HouseHooks";
import { House } from "../types/house";
import HouseForm from "./HouseForm";

const HouseAdd = () => {
    const addHouseMutation = useAddHouse();

    const house: House = {
        address: "",
        country: "",
        description: "",
        price: 0,
        photo: "",
        id: 0
    }

    return (
        <>
            {addHouseMutation.isError && (
                <ValidationSummary error={addHouseMutation.error}/>
            )}
            <HouseForm
                house={ house }
                submitted={(h) => addHouseMutation.mutate(h)}
            />
        </>
    );
}

export default HouseAdd;