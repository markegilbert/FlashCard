import navValues from "../helpers/NavValues";
import { useContext } from "react";
import FlashCardContext from "../Helpers/FlashCardContext";


const Navbar = () => {

	// Destructure only the properties from FlashCardContext that I need in this component
	const { currentNavLocation, currentTopicId, navigate } = useContext(FlashCardContext);

	switch (currentNavLocation) {
		case navValues.study:
			return (
				<>
					<link href="/css/Navbar.css" rel="stylesheet" />
					<div className="navbar">
						<ul>
							<li><div>Study</div></li>
							<li><a href="#" onClick={() => navigate(navValues.manage, currentTopicId)}>Manage</a></li>
						</ul>
					</div>
				</>
			);
		case navValues.manage:
			return (
				<>
					<link href="/css/Navbar.css" rel="stylesheet" />
					<div className="navbar">
						<ul>
							<li><a href="#" onClick={() => navigate(navValues.study, currentTopicId)}>Study</a></li>
							<li><div>Manage</div></li>
						</ul>
					</div>
				</>
			);
		default:
			return (
				<span>No component for navigation value &quot;{currentNavLocation}&quot; found.</span>
			);
	}
};


export default Navbar;
