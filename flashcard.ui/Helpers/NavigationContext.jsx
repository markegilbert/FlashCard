import navValues from "../Helpers/navValues";
import { createContext } from "react";

const NavigationContext = createContext({
    currentNavLocation: navValues.study,
    navigate: () => { }
});


export default NavigationContext;