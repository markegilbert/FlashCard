import { useContext } from "react";
import navValues from "../helpers/NavValues";
import FlashCardContext from "../Helpers/FlashCardContext";
import FlashCardList from "./FlashCardList"
//import ManageFlashCardList from "./ManageFlashCardList";


const ViewSelector = () => {

	// Destructure only the properties from FlashCardContext that I need in this component
	const { currentNavLocation, currentTopicId } = useContext(FlashCardContext);

	switch (currentNavLocation) {
		case navValues.study:
			return (
				<FlashCardList topicID={currentTopicId} />
			);
		case navValues.manage:
			return (
				//<ManageFlashCardList topicID={currentTopicId} />
				<h3>ManageFlashCardList for topic {currentTopicId}</h3>
			);
		default:
			return (
				<span>No screen for navigation value &quot;{currentNavLocation}&quot; found.</span>
			);
	}
};


export default ViewSelector;
