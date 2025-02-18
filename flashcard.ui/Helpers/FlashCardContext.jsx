import navValues from "./navValues";
import { createContext } from "react";

const FlashCardContext = createContext({
    currentNavLocation: navValues.study,
    currentTopicId: "",
    navigate: () => { }
});


export default FlashCardContext;