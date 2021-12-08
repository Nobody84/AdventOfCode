package assert

import "testing"

func Equal(t *testing.T, answer int, solution int, description string) {
	if answer != solution {
		t.Errorf("[%s] The answer was %d but should be %d.", description, answer, solution)
	}
}
