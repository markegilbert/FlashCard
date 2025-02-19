import { useContext } from "react";
import navValues from "../helpers/NavValues";
import FlashCardContext from "../Helpers/FlashCardContext";
import FlashCardList from "./FlashCardList"
import ManageFlashCardList from "./ManageFlashCardList";


const ViewSelector = () => {

	// Destructure only the properties from FlashCardContext that I need in this component
	const { currentNavLocation, currentTopicId } = useContext(FlashCardContext);

	switch (currentNavLocation) {
		case navValues.study:
			return (
				// The selected topic is passed to FlashCardList via context, so no prop is needed here
				<FlashCardList />
			);
		case navValues.manage:
			return (
				// The selected topic is passed to ManageFlashCardList via a prop
				<ManageFlashCardList topicID={currentTopicId} />
			);
		default:
			return (
				<span>No screen for navigation value &quot;{currentNavLocation}&quot; found.</span>
			);
	}
};


export default ViewSelector;
