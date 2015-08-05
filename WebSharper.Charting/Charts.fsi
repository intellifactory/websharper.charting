﻿namespace WebSharper.Charting

module Charts = begin
    
    /// Represents a segment in a polar-based chart (e.g. pie chart).
    type PolarData =
        {
            Value     : float
            Color     : Pervasives.Color
            Highlight : Pervasives.Color
            Label     : string
        }
    
    type DataType<'T> =
        | Static of seq<'T>
        | Live of System.IObservable<'T>

    module DataType = begin
        val Map : fn:('a -> 'b) -> dt:DataType<'a> -> DataType<'b>
    end

    type ChartConfig =
        {
            Title: string
        }

    type SeriesChartConfig =
        {
            XAxis: string
            YAxis: string
            FillColor: Pervasives.Color
            StrokeColor: Pervasives.Color
        }

    type ColorConfig =
        {
            PointColor: Pervasives.Color
            PointHighlightFill: Pervasives.Color
            PointHighlightStroke: Pervasives.Color
            PointStrokeColor: Pervasives.Color
        }

    type IChart<'Self when 'Self :> IChart<'Self>> =
        interface
            abstract member WithTitle : string -> 'Self
            abstract member Config : ChartConfig
        end

    type IColorChart<'Self when 'Self :> IColorChart<'Self>> =
        interface
            inherit IChart<'Self>

            abstract member WithPointColor : Pervasives.Color -> 'Self
            abstract member WithPointHighlightFill : Pervasives.Color -> 'Self
            abstract member WithPointHighlightStroke : Pervasives.Color -> 'Self
            abstract member WithPointStrokeColor : Pervasives.Color -> 'Self
            abstract member ColorConfig : ColorConfig
        end

    type ISeriesChart<'Self when 'Self :> ISeriesChart<'Self>> =
        interface
            inherit IChart<'Self>

            abstract member WithFillColor : Pervasives.Color -> 'Self
            abstract member WithStrokeColor : Pervasives.Color -> 'Self
            abstract member WithXAxis : string -> 'Self
            abstract member WithYAxis : string -> 'Self
            abstract member SeriesConfig : SeriesChartConfig
        end

    type LineChart =
        class
            interface IColorChart<LineChart>
            interface ISeriesChart<LineChart>
            interface IChart<LineChart>
        
            member WithFillColor : color:Pervasives.Color -> LineChart
            member WithPointColor : color:Pervasives.Color -> LineChart
            member WithPointHighlightFill : color:Pervasives.Color -> LineChart
            member WithPointHighlightStroke : color:Pervasives.Color -> LineChart
            member WithPointStrokeColor : color:Pervasives.Color -> LineChart
            member WithStrokeColor : color:Pervasives.Color -> LineChart
            member WithTitle : title:string -> LineChart
            member WithXAxis : xAxis:string -> LineChart
            member WithYAxis : yAxis:string -> LineChart
            member ColorConfig : ColorConfig
            member Config : ChartConfig
            member DataSet : DataType<string * float>
            member SeriesConfig : SeriesChartConfig
        end

    type BarChart =
        class
            interface ISeriesChart<BarChart>
        
            member WithFillColor : color:Pervasives.Color -> BarChart
            member WithStrokeColor : color:Pervasives.Color -> BarChart
            member WithTitle : title:string -> BarChart
            member WithXAxis : xAxis:string -> BarChart
            member WithYAxis : yAxis:string -> BarChart
            member Config : ChartConfig
            member DataSet : DataType<string * float>
            member SeriesConfig : SeriesChartConfig
        end

    type RadarChart =
        class
            interface IColorChart<RadarChart>
            interface ISeriesChart<RadarChart>
            interface IChart<RadarChart>
        
            member WithFillColor : color:Pervasives.Color -> RadarChart
            member WithPointColor : color:Pervasives.Color -> RadarChart
            member WithPointHighlightFill : color:Pervasives.Color -> RadarChart
            member WithPointHighlightStroke : color:Pervasives.Color -> RadarChart
            member WithPointStrokeColor : color:Pervasives.Color -> RadarChart
            member WithStrokeColor : color:Pervasives.Color -> RadarChart
            member WithTitle : title:string -> RadarChart
            member WithXAxis : xAxis:string -> RadarChart
            member WithYAxis : yAxis:string -> RadarChart
            member ColorConfig : ColorConfig
            member Config : ChartConfig
            member DataSet : DataType<string * float>
            member SeriesConfig : SeriesChartConfig
        end

    type IPolarAreaChart<'Self when 'Self :> IPolarAreaChart<'Self>> =
        interface
            inherit IChart<'Self>

            abstract member DataSet : DataType<PolarData>
        end

    type PolarAreaChart =
        class
           interface IPolarAreaChart<PolarAreaChart>
        
            member WithTitle : title:string -> PolarAreaChart
            member Config : ChartConfig
            member DataSet : DataType<PolarData>
        end

    type PieChart =
        class
            interface IPolarAreaChart<PieChart>
        
            member WithTitle : title:string -> PieChart
            member Config : ChartConfig
            member DataSet : DataType<PolarData>
        end

    type DoughnutChart =
        class
            interface IPolarAreaChart<DoughnutChart>
        
            member WithTitle : title:string -> DoughnutChart
            member Config : ChartConfig
            member DataSet : DataType<PolarData>
        end

    type CompositeChart<'T when 'T :> IChart<'T>> =
        class
            member Charts : seq<'T>
        end

    /// Shorthand functions for creating static
    type Chart =
        class
            /// Creates a new bar chart from a sequence of
            /// string and floating-point number pairs.
            static member Bar : dataset:seq<string * float> ->BarChart
            
            /// Combines the given charts into one.
            static member Combine : charts:seq<'a> ->CompositeChart<'a> when 'a :>IChart<'a>

            /// Creates a new doughnut chart (special pie chart) from a
            /// sequence of polar data.
            static member Doughnut : dataset:seq<PolarData> ->DoughnutChart

            /// Creates a new doughnut chart from a sequence of
            /// string and floating-point number pairs.
            static member Doughnut : dataset:seq<string * float> ->DoughnutChart

            /// Creates a new line chart from a sequence of
            /// string and floating-point number pairs.
            static member Line : dataset:seq<string * float> ->LineChart
        
            /// Creates a new pie chart from a
            /// sequence of polar data.
            static member Pie : dataset:seq<PolarData> ->PieChart
            
            /// Creates a new pie chart from a sequence of
            /// string and floating-point number pairs.
            static member Pie : dataset:seq<string * float> ->PieChart
            
            /// Creates a new polar-area chart from a
            /// sequence of polar data.
            static member PolarArea : dataset:seq<PolarData> ->PolarAreaChart

            /// Creates a new polar-area chart from a sequence of
            /// string and floating-point number pairs.
            static member PolarArea : dataset:seq<string * float> ->PolarAreaChart
          
            /// Creates a new radar chart from a sequence of
            /// string and floating-point number pairs.
            static member Radar : dataset:seq<string * float> ->RadarChart
        end

    type LiveChart =
        class
            /// Creates a new bar chart from a continuous flow of
            /// string and floating-point number pairs.
            static member Bar : dataset:System.IObservable<string * float> ->BarChart
            
            /// Creates a new bar chart from a continuous flow of
            /// floating-point numbers.
            static member Bar : dataset:System.IObservable<float> ->BarChart

            /// Creates a new doughnut chart from a continuous flow of
            /// polar data.
            static member Doughnut : dataset:System.IObservable<PolarData> ->DoughnutChart
            
            /// Creates a new doughnut chart from a continuous flow of
            /// string and floating-point number pairs.
            static member Doughnut : dataset:System.IObservable<string * float> ->DoughnutChart
            
            /// Creates a new line chart from a continuous flow of
            /// string and floating-point number pairs.
            static member Line : dataset:System.IObservable<string * float> ->LineChart

            /// Creates a new line chart from a continuous flow of
            /// floating-point numbers.
            static member Line : dataset:System.IObservable<float> ->LineChart

            /// Creates a new pie chart from a continuous flow of
            /// polar data.
            static member Pie : dataset:System.IObservable<PolarData> ->PieChart
            
            /// Creates a new pie chart from a continuous flow of
            /// string and floating-point number pairs.
            static member Pie : dataset:System.IObservable<string * float> ->PieChart
            
            /// Creates a new polar-area chart from a continuous flow of
            /// polar data.
            static member PolarArea : dataset:System.IObservable<PolarData> ->PolarAreaChart
            
            /// Creates a new polar-area chart from a continuous flow of
            /// string and floating-point number pairs.
            static member PolarArea : dataset:System.IObservable<string * float> ->PolarAreaChart
            
            /// Creates a new radar chart from a continuous flow of
            /// string and floating-point number pairs.
            static member Radar : dataset:System.IObservable<string * float> ->RadarChart
            
            /// Creates a new radar chart from a continuous flow of
            /// floating-point numbers.
            static member Radar : dataset:System.IObservable<float> ->RadarChart
        end
end
