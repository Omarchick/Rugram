import React from "react";
import { storeContext } from "../context/storeContext";

/** Хук для работы со стором */
const UseStores = () => React.useContext(storeContext);

export default UseStores;
