import navValues from "../helpers/NavValues";
import { useContext } from "react";
import NavigationContext from "../Helpers/NavigationContext";
// TODO: Add the imports for addFlashCard and deleteFlashCard


const Navbar = () => {

	// Destructure the navigate function out of the context object
	const { currentNavLocation, navigate } = useContext(NavigationContext);

	switch (currentNavLocation) {
		case navValues.study:
			return (
				<div>
					<span>Study</span>
					<button onClick={() => navigate(navValues.addFlashcard)}>Add</button>
					<button onClick={() => navigate(navValues.deleteFlashcard)}>Delete</button>
				</div>
			);
		case navValues.addFlashcard:
			return (
				<div>
					<button onClick={() => navigate(navValues.study)}>Study</button>
					<span>Add</span>
					<button onClick={() => navigate(navValues.deleteFlashcard)}>Delete</button>
				</div>
			);
		case navValues.deleteFlashcard:
			return (
				<div>
					<button onClick={() => navigate(navValues.study)}>Study</button>
					<button onClick={() => navigate(navValues.addFlashcard)}>Add</button>
					<span>Delete</span>
				</div>
			);
		default:
			return (
				<span>No component for navigation value &quot;{currentNavLocation}&quot; found.</span>
			);
	}
};


export default Navbar;
