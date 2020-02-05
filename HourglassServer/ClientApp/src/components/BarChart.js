import React, { useState } from "react";
import "./BarChart.css";

import { XYPlot, XAxis, YAxis, VerticalBarSeries, Crosshair } from "react-vis";

const formatCrosshairTitle = values => {
   return {
      title: "College",
      value: values[0].x
   };
};

const formatCrosshairItems = values => {
   return values.map(v => {
      return {
         title: "Degrees (2016)",
         value: v.y
      };
   });
};

const BarChart = props => {
   const [crosshairValues, setCrosshairValues] = useState([]);

   const nearestXHandler = value => {
      setCrosshairValues([value]);
   };

   const mouseLeaveHandler = () => {
      setCrosshairValues([]);
   };

   const data = props.Education.Universities.map(college => {
      const name = college.properties.NAME;
      return {
         // The full name of Cal Poly is too long for a bar chart
         name,
         x: name.includes("Polytechnic") ? "Cal Poly San Luis Obispo" : name,
         y: college.properties.NUM_DEGREES
      };
   });

   const barWidth = 100;
   const chartWidth = barWidth * data.length;

   const dataWithColor = data.map(d => ({
      ...d,
      color:
         props.Education.ActiveSchools.length > 0
            ? props.Education.ActiveSchools.find(
                 school => d.name === school.name
              )
               ? "#2969F4"
               : "#96B2F0"
            : "#2969F4"
   }));

   return (
      <XYPlot
         width={chartWidth}
         height={400}
         xType="ordinal"
         onMouseLeave={mouseLeaveHandler}
         margin={{ left: 64, bottom: 60, right: 50 }}
      >
         <XAxis className="x-axis" />
         <YAxis />
         <VerticalBarSeries
            colorType="literal"
            data={dataWithColor}
            onNearestX={nearestXHandler}
         />
         <Crosshair
            values={crosshairValues}
            titleFormat={formatCrosshairTitle}
            itemsFormat={formatCrosshairItems}
         />
      </XYPlot>
   );
};

export default BarChart;
