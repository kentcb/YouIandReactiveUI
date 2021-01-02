# Errata

* **Page 17** (section 3.4):

  > `http://reactivex.slack.com`

  Should read:

  > `https://reactiveui.net/slack`

* **Page 51** (section 6.5):

  > If `SomeProperty` changes...

  Should read:

  > If `SomeObservable` changes...

* **Page 65** (chapter 8): Closing parenthesis on `ToProperty` invocation is missing

* **Page 73** (section 8.3):

  > `ReactiveCommand<`Unit,Unit>

  Should read:

  > `ReactiveCommand<Unit,Unit>`

  ---

  > `Func<TParam,CancellationToken,TResult>`

  Should read:

  >  `Func<TParam,CancellationToken,Task<TResult>>`

  ---

  > `this.dinosaurs.Remove`

  Should read:

  > `this.dinosaurs.Add`

* **Page 149** (section 17.1):

  > In both cases, I will refer you back to this API diagram at appropriate junctures.

  This sentence is repeated.

* **Page 237** (appendix A):

  Figure A.24 is the same as figure A.23, but it should instead show a terminating observable.