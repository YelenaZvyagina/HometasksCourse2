namespace PointFree

module Functions =

    let step1 x l = List.map (fun y -> y * x) l
    let step2 x = List.map (fun y -> y * x)
    let step3 x = List.map ((*) x)
    let step4 x = x |> (*) |> List.map
    let step5 = (*) >> List.map
