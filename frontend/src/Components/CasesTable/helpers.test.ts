import { sort } from "./helpers"

test("smaller number is placed earlier when ascending", () => {
    expect([8, 5, 3, 6].sort((a, b) => sort(a, b, "ascending"))).toEqual([3, 5, 6, 8])
})

test("smaller number is placed later when descending", () => {
    expect([8, 5, 3, 6].sort((a, b) => sort(a, b, "descending"))).toEqual([8, 6, 5, 3])
})

test("letters earlier in the alphabet is placed earlier when ascending", () => {
    expect(["face", "mask", "hat", "person"].sort((a, b) => sort(a, b, "ascending"))).toEqual(["face", "hat", "mask", "person"])
})

test("letters earlier in the alphabet is placed later when descending", () => {
    expect(["face", "mask", "hat", "person"].sort((a, b) => sort(a, b, "descending"))).toEqual(["person", "mask", "hat", "face"])
})

test("capital letters is sorted earlier than downcase letters when ascending", () => {
    expect(["face", "Mask", "hat", "Person"].sort((a, b) => sort(a, b, "ascending"))).toEqual(["Mask", "Person", "face", "hat"])
})

test("capital letters is sorted after downcase letters when descending", () => {
    expect(["face", "Mask", "hat", "Person"].sort((a, b) => sort(a, b, "descending"))).toEqual(["hat", "face", "Person", "Mask"])
})

test("for booleans, false is placed before true when ascending", () => {
    expect([true, false, false, true].sort((a, b) => sort(a, b, "ascending"))).toEqual([false, false, true, true])
})

test("for booleans, false is placed after true when descending", () => {
    expect([true, false, false, true].sort((a, b) => sort(a, b, "descending"))).toEqual([true, true, false, false])
})

test("sorting handles \"none\" as sortDirection (treats it as descending)", () => {
    expect([8, 5, 3, 6].sort((a, b) => sort(a, b, "none"))).toEqual([8, 6, 5, 3])
})
