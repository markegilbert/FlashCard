import { useState, useCallback } from 'react';
import navValues from "../Helpers/navValues";
import NavigationContext from './NavigationContext';


const NavigationProvider = ({ children }) => {

    // Use a callback so that children are not accessing the setNav function directly.  Allows App,js to maintain control over its state.
    const navigate = useCallback(
        (navTo) => setNav({ currentNavLocation: navTo, navigate }),
        []
    );

    const [nav, setNav] = useState({ currentNavLocation: navValues.study, navigate });


    return (
        <NavigationContext.Provider value={nav}>
            {children}
        </NavigationContext.Provider>
    )
};


export default NavigationProvider;