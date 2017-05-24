module MetaModels

[<Measure>] type percentage;

type Segment = { Segments : string seq; Selected : string }
type ParamOnCurve = { Segment : Segment; Position : float<percentage> }
type Quantity = { Value : float; Unit : string }
type Function = { Position : ParamOnCurve; Value : Quantity }

type ParameterValue = 
    | Quantity      of Quantity
    | Function      of Function seq
    | Socket        of Socket
and Parameter       = { Name: string; Value: ParameterValue }
and Group           = string * Parameter seq
and Configuration   = Group seq
and Socket          = { Choices : Map<string, Configuration>; Selection : string }