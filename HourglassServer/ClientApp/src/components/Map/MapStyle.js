import MAP_STYLE from "./mapStyle.json";

// Make a copy of the map style
const mapStyle = {
   ...MAP_STYLE,
   sources: { ...MAP_STYLE.sources },
   layers: MAP_STYLE.layers.slice()
};

export default mapStyle;
