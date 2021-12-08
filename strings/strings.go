package strings

import "sort"

type sortRunes []rune

func SortStringAsc(s string) string {
	r := []rune(s)
	sort.Sort(sortRunes(r))
	return string(r)
}

func (s sortRunes) Less(i, j int) bool {
	return s[i] < s[j]
}

func (s sortRunes) Swap(i, j int) {
	s[i], s[j] = s[j], s[i]
}

func (s sortRunes) Len() int {
	return len(s)
}
