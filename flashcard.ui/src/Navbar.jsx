import navValues from "../helpers/NavValues";
// import flashCardList from "./FlashCardList";
// TODO: Add the imports for addFlashCard and deleteFlashCard

const Navbar = (currentNavLocation) => {

	switch (currentNavLocation.currentNavLocation) {
		case navValues.study:
			//return <span>Study</span>;
			return (
				<div>
					<span>Study</span>
					<button>Add</button>
					<button>Delete</button>
				</div>
			);
		case navValues.addFlashcard:
			//return <span>Add</span>;
			return (
				<div>
					<button>Study</button>
					<span>Add</span>
					<button>Delete</button>
				</div>
			);
		case navValues.deleteFlashcard:
			//return <span>Delete</span>;
			return (
				<div>
					<button>Study</button>
					<button>Add</button>
					<span>Delete</span>
				</div>
			);
		default:
			return (
				<span>No component for navigation value &quot;{currentNavLocation.current}&quot; found.</span>
			);
	}
};


export default Navbar;
