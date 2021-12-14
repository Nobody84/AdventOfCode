package assert

import "testing"

func Equal(t *testing.T, answer int, solution int, description string) {
	if answer != solution {
		t.Errorf("[%s] The answer was %d but should be %d.", description, answer, solution)
		t.FailNow()
	}
}

func True(t *testing.T, answer bool, description string) {
	if !answer {
		t.Errorf(description)
		t.FailNow()
	}
}
