import navValues from "./NavValues";
import { createContext } from "react";

const FlashCardContext = createContext({
    currentNavLocation: navValues.study,
    currentTopicId: "",
    navigate: () => { }
});


export default FlashCardContext;