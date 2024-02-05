import { FC } from "react";
import { useParams } from "react-router-dom";

const ItemPage: FC = ()=> {

    const {id} = useParams();
    return (<h1>Item {id}</h1>)
}

export default ItemPage;