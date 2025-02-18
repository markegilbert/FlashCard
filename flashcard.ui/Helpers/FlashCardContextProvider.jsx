import { useState, useCallback } from 'react';
import navValues from "./navValues";
import FlashCardContext from './FlashCardContext';


const FlashCardContextProvider = ({ children }) => {

    // Use a callback so that children are not accessing the setNav function directly.  Allows App,js to maintain control over its state.
    const navigate = useCallback(
        (navTo, topicId) => setNav({ currentNavLocation: navTo, currentTopicId: topicId, navigate }),
        []
    );

    const [nav, setNav] = useState({ currentNavLocation: navValues.study, currentTopicId: "", navigate });


    return (
        <FlashCardContext.Provider value={nav}>
            {children}
        </FlashCardContext.Provider>
    )
};


export default FlashCardContextProvider;