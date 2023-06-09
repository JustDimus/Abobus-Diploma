import { render, screen, cleanup } from "@testing-library/react"
import MapContainer from "../main/MapContainer"

test("render map container", () => {
    render(<MapContainer />);

    const mapContainerElement = screen.getByTestId('todo-1');
    expect(mapContainerElement).toBeInTheDocument();
});